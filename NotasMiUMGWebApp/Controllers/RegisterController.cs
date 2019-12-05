using System.Linq;
using System.Threading.Tasks;
using jguerra.notasmiumg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotasMiUMGWebApp.Data;
using NotasMiUMGWebApp.Models;

namespace NotasMiUMGWebApp.Controllers
{
    [Route("api/estudiante")]
    [AllowAnonymous]
    public class RegisterController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public RegisterController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);
            if(user != null)
            {
                JsonResult resp = Json(new { 
                    status = 400, 
                    message = "No se puedo guardar el usuario" ,
                    error  = "Nombre de usuario repetido"
                });
                resp.StatusCode = 400;
                return resp;
            }

            var student = _context.Estudiantes.FirstOrDefault(e => e.Carne == model.Carne);
            if(student != null)
            {
                JsonResult resp = Json(new { 
                    status = 400, 
                    message = "No se pudo guardar el estudiante",
                    error = "Carné duplicado"
                });
                resp.StatusCode = 400;
                return resp;
            }

            var pensum = _context.Pensums.FirstOrDefault(p => p.CodigoCarrera == model.CodigoCarrera && p.AnoPensum == model.AnoPensum);
            if(pensum == null)
            {
                JsonResult resp = Json(new { 
                    status = 400,
                    message = "No se pudo guardar el estudiante",
                    error = "El pensum no existe"
                });
                resp.StatusCode = 400;
                return resp;
            }

            var identityUser = await _userManager.CreateAsync(new IdentityUser { Email = model.Username }, model.Password);
            if (!identityUser.Succeeded)
            {
                JsonResult resp = Json(new { 
                    status = 400,
                    message = "No se puedo crear el usuario",
                    error = string.Join(", ", identityUser.Errors)
                });
                resp.StatusCode = 400;
                return resp;
            }

            user = await _userManager.FindByNameAsync(model.Username);
            student = new Estudiante(model.Carne, model.Nombre, model.Apellido, model.AnoInicio, pensum, user);

            await _context.Estudiantes.AddAsync(student);
            await _context.SaveChangesAsync();

            student = _context.Estudiantes.FirstOrDefault(e => e.Carne == model.Carne);


            return Ok(new
            {
                status = 200,
                message = "Estudiante creado exitosamente",
                data = new { student.EstudianteId }
            });
        }


    }
}