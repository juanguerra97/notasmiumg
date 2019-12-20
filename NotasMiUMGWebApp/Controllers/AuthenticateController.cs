using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using NotasMiUMGWebApp.Models.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using NotasMiUMGWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace NotasMiUMGWebApp.Controllers
{
    [Route("api/user")]
    [AllowAnonymous]
    public class AuthenticateController : Controller
    {

        private static readonly Regex REGEX_CORREO_UMG = new Regex(@"^([a-zA-Z0-9ñÑáéíóúÁÉÍÓÚ_+-]+)(@miumg.edu.gt)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private UserManager<IdentityUser> _userManager;

        private ApplicationDbContext _context;

        public AuthenticateController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var usuarioCorreoMatch = REGEX_CORREO_UMG.Match(model.Username);
            if(!usuarioCorreoMatch.Success)
            {
                return Unauthorized(new { message = "No se pudo iniciar sesión", error = "Credenciales inválidas" });
            }

            string username = usuarioCorreoMatch.Groups[1].Value;
            
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("voldemornotienenariz"));
                
                var token = new JwtSecurityToken(
                    issuer: "https://localhost:44321",
                    audience: "https://localhost:44321",
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                var roles = await _userManager.GetRolesAsync(user);
                if(await _userManager.IsInRoleAsync(user,"ESTUDIANTE"))
                {
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        roles = roles,
                        expiration = token.ValidTo,
                        estudianteId = (await _context.Estudiantes.FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == user.Id))?.EstudianteId
                    });
                }

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    roles = roles,
                    expiration = token.ValidTo
                });
            }
            return Unauthorized(new { message = "No se pudo iniciar sesión", error = "Credenciales inválidas" });
        }

    }
}