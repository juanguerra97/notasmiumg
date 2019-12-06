using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using jguerra.notasmiumg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasMiUMGWebApp.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace NotasMiUMGWebApp.Controllers
{

    [Route("api/pensums")]
    public class PensumsController : Controller
    {

        private readonly Regex DUP_KEY_ERROR_REGEX = new Regex(@"^Duplicate entry '.*' for key '.*'.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex CARRERA_FK_ERROR_REGEX = new Regex(@"^Cannot add or update a child row: a foreign key constraint fails.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PensumsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("{CodigoCarrera}")]
        public async Task<IActionResult> GetAllByCarrera([FromRoute] uint codigoCarrera)
        {
            return Ok(new { 
                status = 200,
                message = "Pensums",
                data = new
                {
                    codigoCarrera,
                    pensums = _context.Pensums
                        .Where(p => p.CodigoCarrera == codigoCarrera)
                        .Select(p => p.AnoPensum )
                }
            });
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Crear([FromBody] Pensum model)
        {
            if (!(await esAdmin()))
            {
                return Forbid();
            }

            try
            {
                //var carrera = await _context.Carreras.FirstOrDefaultAsync(c => c.CodigoCarrera == model.CodigoCarrera);
                //model.Carrera = carrera;
                _context.Pensums.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    status = 200,
                    message = "Se guardó el pensum"
                });               
                
            } catch(DbUpdateException ex)
            {

                string error = ex.InnerException.Message;
                if(DUP_KEY_ERROR_REGEX.IsMatch(error))
                {
                    error = "Pensum duplicado";
                } else if (CARRERA_FK_ERROR_REGEX.IsMatch(error))
                {
                    error = "La carrera no existe";
                }
                return BadRequest(new
                {
                    status = 400,
                    message = "No se pudo guardar el pensum",
                    error
                });
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("{CodigoCarrera}/{AnoPensum}")]
        public async Task<IActionResult> Eliminar([FromRoute] uint codigoCarrera, [FromRoute] uint anoPensum)
        {
           
            if (!(await esAdmin()))
            {
                return Forbid();
            }
            
            try
            {
                var pensum = await _context.Pensums
                        .FirstOrDefaultAsync(p => p.CodigoCarrera == codigoCarrera && p.AnoPensum == anoPensum);
                if(pensum == null)
                {
                    return BadRequest(new { 
                        status = 200,
                        message = "No se pudo eliminar el pensum",
                        error = "El pensum no existe"
                    });
                }
                _context.Pensums.Remove(pensum);
                await _context.SaveChangesAsync();
                return Ok(new { 
                    status = 200,
                    message = "Se eliminó el pensum"
                });
            } catch (DbUpdateException ex)
            {
                string error = ex.InnerException.Message;
                return BadRequest(new {
                    status = 400,
                    message = "No se pudo eliminar el pensum",
                    error
                }); ;
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