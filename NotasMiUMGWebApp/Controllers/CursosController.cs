using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotasMiUMGWebApp.Data;
using System.Text.RegularExpressions;
using jguerra.notasmiumg;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace NotasMiUMGWebApp.Controllers
{

    [Route("api/cursos")]
    public class CursosController : Controller
    {

        private readonly Regex DUP_KEY_ERROR_REGEX = new Regex(@"^Duplicate entry '.*' for key '(.*)'.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex FK_ERROR_REGEX = new Regex(@"^Cannot delete or update a parent row: a foreign key constraint fails.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CursosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{CodigoCarrera}")]
        public async Task<IActionResult> GetAllByCarrera([FromRoute] uint codigoCarrera)
        {

            return Ok(new { 
                status = 200,
                message = $"Cursos de {codigoCarrera}",
                data = _context.Cursos.Where(c => c.CodigoCarrera == codigoCarrera)
                    .Select(c => new { 
                        c.CodigoCurso,
                        c.NombreCurso
                    })
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{CodigoCarrera}/{AnoPensum}")]
        public async Task<IActionResult> GetAllByCarreraPensum([FromRoute] uint codigoCarrera, [FromRoute] uint anoPensum)
        {

            return Ok(new
            {
                status = 200,
                message = $"Cursos de {codigoCarrera}",
                data = _context.PensumCursos.Where(pc => pc.CodigoCarrera == codigoCarrera && pc.AnoPensum == anoPensum)
                    .Select(pc => new
                    {
                        pc.CodigoCurso,
                        pc.Curso.NombreCurso
                    })
            });
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("{CodigoCarrera}/{AnoPensum}/{Ciclo}")]
        public async Task<IActionResult> GetAllByCarreraPensumCiclo([FromRoute] uint codigoCarrera, [FromRoute] uint anoPensum, [FromRoute] byte ciclo)
        {

            return Ok(new
            {
                status = 200,
                message = $"Cursos de {codigoCarrera}",
                data = _context.PensumCursos.Where(pc => pc.CodigoCarrera == codigoCarrera && pc.AnoPensum == anoPensum && pc.Ciclo == ciclo)
                    .Select(pc => new
                    {
                        pc.CodigoCurso,
                        pc.Curso.NombreCurso
                    })
            });
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Crear([FromBody] Curso model)
        {

            if (!(await esAdmin()))
            {
                return Forbid();
            }

            try
            {
                await _context.Cursos.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Curso guardado"
                });
            }
            catch (DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                MatchCollection groups = DUP_KEY_ERROR_REGEX.Matches(error);

                if (groups.Count > 0)
                {
                    var match = groups[0];
                    if (string.Compare(match.Groups[1].Value, "PRIMARY") == 0)
                    {
                        error = "Codigo duplicado";
                    }
                    else
                    {
                        error = "Nombre duplicado";
                    }
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se guardó el curso",
                    error
                });
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{CodigoCarrera}/{CodigoCurso}")]
        public async Task<IActionResult> Eliminar([FromRoute] uint codigoCarrera, [FromRoute] uint codigoCurso)
        {

            if (!(await esAdmin()))
            {
                return Forbid();
            }

            try
            {
                var curso = await _context.Cursos
                    .FirstOrDefaultAsync(c => c.CodigoCarrera == codigoCarrera && c.CodigoCurso == codigoCurso);
                if (curso == null)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        message = "No se pudo eliminar el curso",
                        error = "El curso no existe"
                    });
                }
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Curso eliminado"
                });
            }
            catch (DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                if (FK_ERROR_REGEX.IsMatch(error))
                {
                    error = "Otros registros referencian a este curso";
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo eliminar el curso",
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