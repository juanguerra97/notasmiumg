using System.Linq;
using System.Threading.Tasks;
using jguerra.notasmiumg;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using NotasMiUMGWebApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using NotasMiUMGWebApp.Models;

namespace NotasMiUMGWebApp.Controllers
{

    [Route("api/pensumcurso")]
    public class PensumCursosController : Controller
    {

        private readonly Regex DUP_KEY_ERROR_REGEX = new Regex(@"^Duplicate entry '.*' for key '(.*)'.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex FK_ERROR_REGEX = new Regex(@"^Cannot delete or update a parent row: a foreign key constraint fails.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PensumCursosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Crear([FromBody] PensumCurso model)
        {

            if (!(await esAdmin()))
            {
                return Forbid();
            }

            try
            {
                await _context.PensumCursos.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Relacion Pensum-Curso guardada"
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
                        error = "Datos duplicados";
                    }
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se guardó la relación Pensum-Curso",
                    error
                });
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("{CodigoCarrera}/{AnoPensum}/{CodigoCurso}")]
        public async Task<IActionResult> Eliminar([FromRoute] uint codigoCarrera, [FromRoute] uint anoPensum, [FromRoute] uint codigoCurso)
        {

            if (!(await esAdmin()))
            {
                return Forbid();
            }

            try
            {
                var pensumCurso = await _context.PensumCursos
                    .FirstOrDefaultAsync(pc => pc.CodigoCarrera == codigoCarrera && pc.AnoPensum == anoPensum && pc.CodigoCurso == codigoCurso);
                if (pensumCurso == null)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        message = "No se pudo eliminar el Pensum-Curso",
                        error = "No existe la relación Pensum-Curso"
                    });
                }
                _context.PensumCursos.Remove(pensumCurso);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Pensum-Curso eliminado"
                });
            }
            catch (DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                if (FK_ERROR_REGEX.IsMatch(error))
                {
                    error = "Otros registros referencian a esta relación";
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo eliminar el Pensum-Curso",
                    error
                });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Actualizar([FromBody] PensumCursoModel model)
        {

            if (!(await esAdmin()))
            {
                return Forbid();
            }

            try
            {
                var pensumCurso = await _context.PensumCursos
                    .FirstOrDefaultAsync(pc => pc.CodigoCarrera == model.CodigoCarrera && pc.AnoPensum == model.AnoPensum && pc.CodigoCurso == model.CodigoCurso);
                if (pensumCurso == null)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        message = "No se pudo actualizar el Pensum-Curso",
                        error = "No existe la relación Pensum-Curso"
                    });
                }

                pensumCurso.Ciclo = model.Ciclo;
                pensumCurso.Creditos = model.Creditos;

                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Pensum-Curso actualizado"
                });
            }
            catch (DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo eliminar el Pensum-Curso",
                    error
                });
            }
        }

        /// <summary>
        /// Metodo que devuelve true si el usuario que esta haciendo la peticion es administrador
        /// </summary>
        /// <returns>true si es admin, false si no</returns>
        private async Task<bool> esAdmin()
        {
            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            return await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ADMIN");
        }

    }

}