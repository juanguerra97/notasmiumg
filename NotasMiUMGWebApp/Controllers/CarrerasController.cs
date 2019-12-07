using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using jguerra.notasmiumg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasMiUMGWebApp.Data;
using System.Text.RegularExpressions;

namespace NotasMiUMGWebApp.Controllers
{

    [Route("api/carreras")]
    public class CarrerasController : Controller
    {

        private readonly Regex DUP_KEY_ERROR_REGEX = new Regex(@"^Duplicate entry '.*' for key '(.*)'.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex FK_ERROR_REGEX = new Regex(@"^Cannot delete or update a parent row: a foreign key constraint fails.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarrerasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new {
                status = 200,
                message = "Carreras",
                data = _context.Carreras
                    .Select(c => new { c.CodigoCarrera, c.NombreCarrera })
            }); ;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{codigo}")]
        public async Task<IActionResult> Get([FromRoute] uint codigo)
        {
            var carrera = _context.Carreras
                .Select(c => new { c.CodigoCarrera, c.NombreCarrera })
                .FirstOrDefault(c => c.CodigoCarrera == codigo);
            if(carrera != null)
            {
                return Ok(new {
                    status = 200,
                    message = "Carrera",
                    data = carrera
                }); ;
            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "No se encontró la carrera",
                    error = "La carrera no existe"
                });
            }
            
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] Carrera model)
        {

            if (!(await esAdmin()))
            {
                return Forbid();
            }

            //var carrera = await _context.Carreras
            //    .FirstOrDefaultAsync(c => c.CodigoCarrera == model.CodigoCarrera);
            //if(carrera != null)
            //{
            //    return BadRequest(new
            //    {
            //        status = 400,
            //        message = "No se guardó la carrera",
            //        error = "Código duplicado"
            //    });
            //}

            //carrera = await _context.Carreras
            //    .FirstOrDefaultAsync(c => c.NombreCarrera.Equals(model.NombreCarrera, 
            //        StringComparison.InvariantCultureIgnoreCase));
            //if (carrera != null)
            //{
            //    return BadRequest(new
            //    {
            //        status = 400,
            //        message = "No se guardó la carrera",
            //        error = "Nombre duplicado"
            //    });
            //}
            try
            {
                await _context.Carreras.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new { 
                    status = 200,
                    message = "Carrera guardada" });
            } catch(DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                MatchCollection groups = DUP_KEY_ERROR_REGEX.Matches(error);

                if (groups.Count > 0)
                {
                    var match = groups[0];
                    if (string.Compare(match.Groups[1].Value,"PRIMARY") == 0)
                    {
                        error = "Codigo duplicado";
                    } else
                    {
                        error = "Nombre duplicado";
                    }
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se guardó la carrera",
                    error
                });
            }
            
        }

        [Authorize]
        [HttpDelete]
        [Route("{CodigoCarrera}")]
        public async Task<IActionResult> Eliminar([FromRoute] uint codigoCarrera)
        {

            try
            {
                var carrera = await _context.Carreras.FirstOrDefaultAsync(c => c.CodigoCarrera == codigoCarrera);
                if(carrera == null)
                {
                    return BadRequest(new { 
                        status = 400,
                        message = "No se pudo eliminar la carrera",
                        error = "La carrera no existe"
                    });
                }
                _context.Carreras.Remove(carrera);
                await _context.SaveChangesAsync();
                return Ok(new { 
                    status = 200,
                    message = "Carrera eliminada"
                });
            } catch(DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                if (FK_ERROR_REGEX.IsMatch(error))
                {
                    error = "Otros registros referencian a esta carrera";
                }
                return BadRequest(new { 
                    status = 400,
                    message = "No se pudo eliminar la carrera",
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