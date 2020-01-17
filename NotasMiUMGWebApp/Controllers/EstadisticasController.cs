using System;
using System.Collections.Generic;
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
    [Route("api/estadisticas")]
    public class EstadisticasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EstadisticasController(
            ApplicationDbContext context, 
            UserManager<IdentityUser> userManager
        ) {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        [Route("promedios")]        
        public async Task<IActionResult> GetPromedios()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }
            
            // para los promedios, solo se toman en cuenta las notas de cursos aprobados
            var notas = estud.Notas.Where(n => n.Aprobado);

            // promedios por anio
            // se agrupan las notas por anio, luego se toma el promedio de cada grupo
            var promediosAnual = notas.GroupBy(n => n.Ano, (key, values) =>
                        new {
                            ano = key,
                            promedio = values.Average(n => n.NotaFinal)
                        }).OrderBy(r => r.ano);

            // promedios por semestre en cada anio
            // se agrupan las notas por anio, luego se agrupan por semestre(1 o 2) y se calcula el promedio
            var promediosSemestral = notas.GroupBy(n => n.Ano, (key, values) =>
                        new
                        {
                            ano = key,
                            semestres = values.GroupBy(n => (n.PensumCurso.Ciclo + 1) % 2 + 1, (key, values) =>
                                new
                                {
                                    semestre = key,
                                    promedio = values.Average(n => n.NotaFinal)
                                }).OrderBy(r => r.semestre)
                        });

            // promedio mas grande en un semestre
            var maxPromedioSemestral = promediosSemestral.Select(p => 
                    new 
                    { 
                        p.ano, 
                        maxpromedio = p.semestres.Max(s => s.promedio) 
                    }).Max(r => r.maxpromedio);
            

            return Ok(new
            {
                status = 200,
                message = "Promedios",
                data = new
                {
                    general = notas.Average(n => n.NotaFinal), // promedio general
                    anual = promediosAnual,
                    semestral = promediosSemestral,
                    maxanual = promediosAnual // promedio(s) mas grande(s) en un anio
                        .Where(r => r.promedio == promediosAnual.Max(p => p.promedio))
                        .OrderBy(r => r.ano),
                    maxsemestral = promediosSemestral // promedio(s) mas grande(s) en un semestre 
                        .Select(r => new { r.ano, semestres = r.semestres.Where(s => s.promedio == maxPromedioSemestral)
                        .OrderBy(r => r.semestre) })
                        .Where(r => r.semestres.Count() > 0)
                        .OrderBy(r => r.ano)
                }
            });
        }

        [Authorize]
        [HttpGet]
        [Route("examenfinal")]
        public async Task<IActionResult> GetExamenesFinales()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            var notas = estud.Notas;

            byte? maxNotaExamenFinal = notas.Max(n => n.ExamenFinal);
            byte? minNotaExamenFinal = notas.Min(n => n.ExamenFinal);

            return Ok(new { 
                status = 200,
                message = "Estadísticas examenes finales",
                data = new {
                    max = new {
                        val = maxNotaExamenFinal, // examen final mas alto
                        cursos = notas.Where(n => n.ExamenFinal == maxNotaExamenFinal)
                            .Select(n => new {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                    min = new { 
                        val = minNotaExamenFinal, // examen final mas bajo
                        cursos = notas.Where(n => n.ExamenFinal == minNotaExamenFinal)
                            .Select(n => new
                            {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                }
            });
        }

        [Authorize]
        [HttpGet]
        [Route("primerparcial")]
        public async Task<IActionResult> GetPrimerosParciales()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            var notas = estud.Notas;

            byte? maxNotaPrimerParcial = notas.Max(n => n.PrimerParcial);
            byte? minNotaPrimerParcial = notas.Min(n => n.PrimerParcial);

            return Ok(new
            {
                status = 200,
                message = "Estadísticas primeros parciales",
                data = new
                {
                    max = new
                    {
                        val = maxNotaPrimerParcial, // primer parcial mas alto
                        cursos = notas.Where(n => n.PrimerParcial == maxNotaPrimerParcial)
                            .Select(n => new {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                    min = new
                    {
                        val = minNotaPrimerParcial, // primer parcial mas bajo
                        cursos = notas.Where(n => n.PrimerParcial == minNotaPrimerParcial)
                            .Select(n => new
                            {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                }
            });
        }

        [Authorize]
        [HttpGet]
        [Route("segundoparcial")]
        public async Task<IActionResult> GetSegundosParciales()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            var notas = estud.Notas;

            byte? maxNotaSegundoParcial = notas.Max(n => n.SegundoParcial);
            byte? minNotaSegundoParcial = notas.Min(n => n.SegundoParcial);

            return Ok(new
            {
                status = 200,
                message = "Estadísticas segundos parciales",
                data = new
                {
                    max = new
                    {
                        val = maxNotaSegundoParcial, // segundo parcial mas alto
                        cursos = notas.Where(n => n.SegundoParcial == maxNotaSegundoParcial)
                            .Select(n => new {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                    min = new
                    {
                        val = minNotaSegundoParcial, // segundo parcial mas bajo
                        cursos = notas.Where(n => n.SegundoParcial == minNotaSegundoParcial)
                            .Select(n => new
                            {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                }
            });
        }

        [Authorize]
        [HttpGet]
        [Route("actividades")]
        public async Task<IActionResult> GetActividades()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            var notas = estud.Notas;

            byte? maxNotaActividades = notas.Max(n => n.Actividades);
            byte? minNotaActividades = notas.Min(n => n.Actividades);

            return Ok(new
            {
                status = 200,
                message = "Estadísticas actividades",
                data = new
                {
                    max = new
                    {
                        val = maxNotaActividades, // nota mas alta en actividades
                        cursos = notas.Where(n => n.Actividades == maxNotaActividades)
                            .Select(n => new {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                    min = new
                    {
                        val = minNotaActividades, // nota mas baja en actividades
                        cursos = notas.Where(n => n.Actividades == minNotaActividades)
                            .Select(n => new
                            {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                }
            });
        }

        [Authorize]
        [HttpGet]
        [Route("zona")]
        public async Task<IActionResult> GetZonas()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            var notas = estud.Notas;

            byte? maxZona = notas.Max(n => n.Zona);
            byte? minZona = notas.Min(n => n.Zona);

            return Ok(new
            {
                status = 200,
                message = "Estadísticas de zonas",
                data = new
                {
                    max = new
                    {
                        val = maxZona, // zona mas alta
                        cursos = notas.Where(n => n.Zona == maxZona)
                            .Select(n => new {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                    min = new
                    {
                        val = minZona, // zona mas baja
                        cursos = notas.Where(n => n.Zona == minZona)
                            .Select(n => new
                            {
                                n.PensumCurso.Curso.NombreCurso,
                                n.Ano,
                                n.Aprobado
                            })
                    },
                }
            });
        }

        [Authorize]
        [HttpGet]
        [Route("creditos")]
        public async Task<IActionResult> GetCreditos()
        {

            var idUsuario = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            bool esEstud = await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(idUsuario), "ESTUDIANTE");
            var estud = await _context.Estudiantes.Include(e => e.Notas).ThenInclude(n => n.PensumCurso)
                .FirstOrDefaultAsync(e => e.UsuarioEstudiante.Id == idUsuario);
            if (!esEstud || estud == null)
            {
                return Forbid();
            }

            var notas = estud.Notas.Where(n => n.Aprobado);

            return Ok(new
            {
                status = 200,
                message = "Estadísticas de créditos",
                data = new
                {
                    total = notas.Sum(n => n.PensumCurso.Creditos) // total de creditos ganados con los cursos aprobados
                }
            });
        }

    }
}