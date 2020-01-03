using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasMiUMGWebApp.Data;

namespace NotasMiUMGWebApp.Controllers
{
    [Route("api/estadisticas")]
    public class EstadisticasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EstadisticasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        [Route("promedios")]        
        public async Task<IActionResult> GetPromedios()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            var promediosAnual = estud.Notas.GroupBy(n => n.Ano, (key, values) =>
                        new {
                            ano = key,
                            promedio = values.Average(n => n.NotaFinal)
                        }).OrderBy(r => r.ano);

            var promediosSemestral = estud.Notas.GroupBy(n => n.Ano, (key, values) =>
                        new
                        {
                            ano = key,
                            semestres = values.GroupBy(n => (n.PensumCurso.Ciclo + 1) % 2 + 1, (key, values) =>
                                new
                                {
                                    semestre = key,
                                    promedio = values.Average(n => n.NotaFinal)
                                }).OrderBy(r => r.semestre)
                        });

            var maxPromedioSemestral = promediosSemestral.Select(p => new { p.ano, maxpromedio = p.semestres.Max(s => s.promedio) }).Max(r => r.maxpromedio);
            

            return Ok(new
            {
                status = 200,
                message = "Promedios",
                data = new
                {
                    general = estud.Notas.Average(n => n.NotaFinal),
                    anual = promediosAnual,
                    semestral = promediosSemestral,
                    maxanual = promediosAnual.Where(r => r.promedio == promediosAnual.Max(p => p.promedio)).OrderBy(r => r.ano),
                    maxsemestral = promediosSemestral.Select(r => new { r.ano, semestres = r.semestres.Where(s => s.promedio == maxPromedioSemestral).OrderBy(r => r.semestre) }).Where(r => r.semestres.Count() > 0).OrderBy(r => r.ano)
                }
            });
        }

    }
}