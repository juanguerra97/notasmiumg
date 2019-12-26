using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotasMiUMGWebApp.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using jguerra.notasmiumg;

namespace NotasMiUMGWebApp.Controllers
{
    [Route("api/notas")]
    public class NotasController : Controller
    {

        private readonly Regex DUP_KEY_ERROR_REGEX = new Regex(@"^Duplicate entry '.*' for key '(.*)'.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex FK_ERROR_REGEX = new Regex(@"^Cannot delete or update a parent row: a foreign key constraint fails.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NotasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            return Ok(new { 
                status = 200,
                message = "Notas",
                data = _context.Notas.Where(n => n.Estudiante.EstudianteId == estud.EstudianteId)
                    .OrderBy(n => n.Ano).ThenBy(n => n.PensumCurso.Ciclo)
                    .Select(n => new
                    {
                        n.Ano, 
                        n.PensumCurso.Ciclo,
                        n.CodigoCarrera,
                        n.CodigoCurso,
                        n.PensumCurso.Curso.NombreCurso,
                        n.PrimerParcial,
                        n.SegundoParcial,
                        n.Actividades,
                        n.Zona,
                        n.ExamenFinal,
                        n.NotaFinal,
                        n.Aprobado
                    })
            });
        }

        [Authorize]
        [HttpGet]
        [Route("{Ano}")]
        public async Task<IActionResult> GetAllByAno([FromRoute] uint ano)
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            return Ok(new
            {
                status = 200,
                message = "Notas",
                data = _context.Notas.Where(n => n.EstudianteId == estud.EstudianteId && n.Ano == ano)
                    .OrderBy(n => n.PensumCurso.Ciclo)
                    .Select(n => new
                    {
                        n.PensumCurso.Ciclo,
                        n.CodigoCarrera,
                        n.CodigoCurso,
                        n.PensumCurso.Curso.NombreCurso,
                        n.PrimerParcial,
                        n.SegundoParcial,
                        n.Actividades,
                        n.Zona,
                        n.ExamenFinal,
                        n.NotaFinal,
                        n.Aprobado
                    })
            });
        }

        [Authorize]
        [HttpGet]
        [Route("{Ano}/{Ciclo}")]
        public async Task<IActionResult> GetAllByAnoCiclo([FromRoute] uint ano, [FromRoute] byte ciclo)
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            return Ok(new
            {
                status = 200,
                message = "Notas",
                data = _context.Notas.Where(n => n.EstudianteId == estud.EstudianteId && n.Ano == ano && n.PensumCurso.Ciclo == ciclo)
                    .Select(n => new
                    {
                        n.CodigoCarrera,
                        n.CodigoCurso,
                        n.PensumCurso.Curso.NombreCurso,
                        n.PrimerParcial,
                        n.SegundoParcial,
                        n.Actividades,
                        n.Zona,
                        n.ExamenFinal,
                        n.NotaFinal,
                        n.Aprobado
                    })
            });
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Crear([FromBody] Nota model)
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(
                await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Pensum)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null || model.EstudianteId != estud.EstudianteId)
            {
                return Forbid();
            }

            try
            {
                await _context.Notas.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Nota guardada"
                });
            } catch(DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                MatchCollection groups = DUP_KEY_ERROR_REGEX.Matches(error);

                if (groups.Count > 0)
                {
                    var match = groups[0];
                    if (string.Compare(match.Groups[1].Value, "PRIMARY") == 0)
                    {
                        error = "Nota duplicada";
                    }
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se guardó la nota",
                    error
                });
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{CodigoCurso}/{Ano}")]
        public async Task<IActionResult> Eliminar([FromRoute] uint codigoCurso, [FromRoute] uint ano)
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(
                await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Pensum)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            try
            {
                var nota = await _context.Notas
                    .FirstOrDefaultAsync(n => n.EstudianteId == estud.EstudianteId 
                        && n.CodigoCarrera == estud.Pensum.CodigoCarrera 
                        && n.AnoPensum == estud.Pensum.AnoPensum && n.CodigoCurso == codigoCurso && n.Ano == ano);
                if(nota == null)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        message = "No se pudo eliminar la nota",
                        error = "La nota no existe"
                    });
                }
                _context.Notas.Remove(nota);
                await _context.SaveChangesAsync();
                return Ok(new { 
                    status = 200,
                    message = "Se eliminó la nota"
                });
            } catch(DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                if (FK_ERROR_REGEX.IsMatch(error))
                {
                    error = "Otros registros referencian a esta nota";
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo eliminar la nota",
                    error
                });
            }

        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Actualizar([FromBody] Nota model)
        {
            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(
                await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Pensum)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            try
            {
                var nota = await _context.Notas
                    .FirstOrDefaultAsync(n => n.EstudianteId == estud.EstudianteId 
                        && n.CodigoCarrera == estud.Pensum.CodigoCarrera
                        && n.AnoPensum == estud.Pensum.AnoPensum && n.CodigoCurso == model.CodigoCurso 
                        && n.Ano == model.Ano);
                if (nota == null)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        message = "No se pudo actualizar la nota",
                        error = "La nota no existe"
                    });
                }
                nota.PrimerParcial = model.PrimerParcial;
                nota.SegundoParcial = model.SegundoParcial;
                nota.Actividades = model.Actividades;
                nota.ExamenFinal = model.ExamenFinal;
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Se actualizó la nota"
                });
            }
            catch (DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo actualizar la nota",
                    error
                });
            }

        }

        /// <summary>
        /// Metodo que devuelve true si el usuario que esta haciendo la peticion es un estudiante y 
        /// el id del usuario del estudiante es igual al id del usuario logueado
        /// </summary>
        /// <returns>true si es estudiante, false si no</returns>
        private async Task<bool> esEstudiante(uint? estudianteId = null)
        {
            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            if(esEstud && estudianteId != null)
            {
                var estud = await _context.Estudiantes.FirstOrDefaultAsync(e => e.EstudianteId == estudianteId);
                string idUsuarioEstud = estud?.UsuarioEstudiante.Id;
                return idUsuario == idUsuarioEstud;
            }
            return esEstud;
        }


    }
}