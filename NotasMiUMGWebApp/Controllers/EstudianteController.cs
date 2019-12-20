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

    [Route("api/estud")]
    public class EstudianteController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;


        public EstudianteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(
                await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            if (!esEstud)
            {
                return Forbid();
            }
            var estud = await _context.Estudiantes
                .Include(e => e.Pensum)
                .Include(e => e.UsuarioEstudiante)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if(estud == null)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudieron obtener los datos",
                    error = "No se encontró el estudiante"
                });
            }
            return Ok(new { 
                status = 200,
                message = "Estudiante",
                data = new
                {
                    estud.EstudianteId,
                    estud.Carne,
                    estud.Nombre,
                    estud.Apellido,
                    correo = estud.UsuarioEstudiante.Email,
                    estud.Pensum.CodigoCarrera,
                    estud.Pensum.AnoPensum,
                    estud.AnoInicio
                }
            });
        }

    }
}