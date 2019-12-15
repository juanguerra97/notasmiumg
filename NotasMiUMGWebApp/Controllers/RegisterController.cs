using System.Linq;
using System.Threading.Tasks;
using jguerra.notasmiumg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using NotasMiUMGWebApp.Data;
using NotasMiUMGWebApp.Models;

namespace NotasMiUMGWebApp.Controllers
{
    [Route("api/estudiante")]
    [AllowAnonymous]
    public class RegisterController : Controller
    {

        private static readonly Regex REGEX_CORREO_UMG = new Regex(@"^([a-zA-Z0-9ñÑ_+-]+)@miumg.edu.gt$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel model)
        {

            var matchCorreo = REGEX_CORREO_UMG.Match(model.correo);
            if (!matchCorreo.Success)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "No se puedo guardar el usuario",
                    error = "Correo inválido"
                });
            }

            var username = matchCorreo.Groups[1].Value;

            var user = await _userManager.FindByNameAsync(username);
            if(user != null)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "No se puedo guardar el usuario",
                    error = "Nombre de usuario duplicado"
                });
            }

            var student = _context.Estudiantes.FirstOrDefault(e => e.Carne == model.carne);
            if(student != null)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo guardar el estudiante",
                    error = "Carné duplicado"
                });
            }

            var pensum = _context.Pensums.FirstOrDefault(p => p.CodigoCarrera == model.codigoCarrera && p.AnoPensum == model.anoPensum);
            if(pensum == null)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo guardar el estudiante",
                    error = "El pensum no existe"
                });
            }

            var identityUser = await _userManager.CreateAsync(new IdentityUser { UserName = username, Email = model.correo }, model.password);
            if (!identityUser.Succeeded)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "No se puedo crear el usuario",
                    error = string.Join(", ", identityUser.Errors)
                });
            }

            var studentRole = await _roleManager.FindByNameAsync("ESTUDIANTE");
            if(studentRole == null)
            {
                studentRole = new IdentityRole { Name = "ESTUDIANTE" };
                var roleResult = await _roleManager.CreateAsync(studentRole);
                if(!roleResult.Succeeded)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        message = "No se pudo crear el rol ESTUDIANTE",
                        error = string.Join(", ", roleResult.Errors)
                    });
                }
            }            

            user = await _userManager.FindByNameAsync(username);

            await _userManager.AddToRoleAsync(user, "ESTUDIANTE");

            student = new Estudiante(model.carne, model.nombre, model.apellido, model.anoInicio, pensum, user);

            await _context.Estudiantes.AddAsync(student);
            await _context.SaveChangesAsync();

            student = _context.Estudiantes.FirstOrDefault(e => e.Carne == model.carne);


            return Ok(new
            {
                status = 200,
                message = "Estudiante creado exitosamente",
                data = new { student.EstudianteId }
            });
        }


    }
}