using System;
using System.Linq;
using System.Threading.Tasks;
using jguerra.notasmiumg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasMiUMGWebApp.Data;

namespace NotasMiUMGWebApp.Controllers
{

    [Route("api/carreras")]
    [AllowAnonymous]
    public class CarrerasController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CarrerasController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        [HttpGet]
        [Route("{codigo}")]
        public async Task<IActionResult> Get([FromRoute] uint codigo)
        {
            var carrera = _context.Carreras.Select(c => new { c.CodigoCarrera, c.NombreCarrera })
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
                JsonResult resp = Json(new {
                    status = 400,
                    message = "No se encontró la carrera",
                    error = "La carrera no existe"
                });
                resp.StatusCode = 400;
                return resp;
            }
            
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] Carrera model)
        {
            var carrera = await _context.Carreras.FirstOrDefaultAsync(c => c.CodigoCarrera == model.CodigoCarrera);
            if(carrera != null)
            {
                JsonResult resp = Json(new { 
                    status = 400,
                    message = "No se guardó la carrera", 
                    error = "Código duplicado" });
                resp.StatusCode = 400;
                return resp;
            }

            carrera = await _context.Carreras.FirstOrDefaultAsync(c => c.NombreCarrera.Equals(model.NombreCarrera, StringComparison.InvariantCultureIgnoreCase));
            if (carrera != null)
            {
                JsonResult resp = Json(new { 
                    status = 400,
                    message = "No se guardó la carrera", 
                    error = "Nombre duplicado" });
                resp.StatusCode = 400;
                return resp;
            }
            try
            {
                await _context.Carreras.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new { 
                    status = 200,
                    message = "Carrera guardada" });
            } catch(DbUpdateException ex)
            {
                JsonResult resp = Json(new { 
                    status = 400,
                    message = "No se guardó la carrera", 
                    error = ex.InnerException.Message });
                resp.StatusCode = 400;
                return resp;
            }
            
        }

    }
}