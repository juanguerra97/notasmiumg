using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using NotasMiUMGWebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotasMiUmg.Authorization;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using jguerra.notasmiumg;

namespace NotasMiUMGWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<IdentityUser, ApplicationDbContext>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddIdentityServerJwt()
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:44321",
                    ValidIssuer = "https://localhost:44321",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("voldemornotienenariz"))
                };
            });
            services.AddControllersWithViews(config =>
            {
                // using Microsoft.AspNetCore.Mvc.Authorization;
                // using Microsoft.AspNetCore.Authorization;
                //var policy = new AuthorizationPolicyBuilder()
                //                 .RequireAuthenticatedUser()
                //                 .Build();
                //config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            
            services.AddScoped<IAuthorizationHandler, AdminAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, EstudianteAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, NotaAuthorizationHandler>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            var scope = app.ApplicationServices.CreateScope();
            await InitRoles(scope.ServiceProvider.GetService<RoleManager<IdentityRole>>());
            await InitAdmin(scope.ServiceProvider.GetService<UserManager<IdentityUser>>());
            await InitData(scope.ServiceProvider.GetService<ApplicationDbContext>());
        }

        // metodo para crear los roles de la aplicacion si no existen
        private static async Task InitRoles(RoleManager<IdentityRole> rm)
        {
            var admin = await rm.FindByNameAsync("ADMIN");
            if(admin == null)
            {
                admin = new IdentityRole("ADMIN");
                await rm.CreateAsync(admin);
            }

            var estudiante = await rm.FindByNameAsync("ESTUDIANTE");
            if(estudiante == null)
            {
                estudiante = new IdentityRole("ESTUDIANTE");
                await rm.CreateAsync(estudiante);
            }

        }

        // metodo para crear al usuario administrador
        private static async Task InitAdmin(UserManager<IdentityUser> um)
        {
            var admin = await um.FindByNameAsync("notasmiumg");

            if(admin == null)
            {
                admin = new IdentityUser("notasmiumg");
                var res = await um.CreateAsync(admin, "notasmiumg");
                if(res.Succeeded)
                {
                    await um.AddToRoleAsync(admin, "ADMIN");
                }
            } else
            {
                if (!(await um.IsInRoleAsync(admin, "ADMIN")))
                {
                    await um.AddToRoleAsync(admin, "ADMIN");
                }
            }
            

        }

        // metodo para guardar algunos datos
        private static async Task InitData(ApplicationDbContext db)
        {
            if(0 < await db.Carreras.CountAsync()) return;

            var sistemas = new Carrera(1490, "INGENIERIA EN SISTEMAS");
            await db.Carreras.AddAsync(sistemas);

            var pensum2014 = new Pensum(2014, sistemas);
            await db.Pensums.AddAsync(pensum2014);

            await db.SaveChangesAsync();

            var pensumCursos = new PensumCurso[]
            {
                new PensumCurso(1, 4, pensum2014, new Curso(1, "DESARROLLO HUMANO Y PROFESIONAL", sistemas)),
                new PensumCurso(1, 5, pensum2014, new Curso(2, "METODOLOGIA DE LA INVESTIGACION", sistemas)),
                new PensumCurso(1, 5, pensum2014, new Curso(3, "CONTABILIDAD I", sistemas)),
                new PensumCurso(1, 5, pensum2014, new Curso(4, "INTRODUCCION A LOS SISTEMAS DE COMPUTO", sistemas)),
                new PensumCurso(1, 5, pensum2014, new Curso(5, "LOGICA DE SISTEMAS", sistemas)),

                new PensumCurso(2, 5, pensum2014, new Curso(6, "PRECALCULO", sistemas)),
                new PensumCurso(2, 5, pensum2014, new Curso(7, "ALGEBRA LINEAL", sistemas)),
                new PensumCurso(2, 5, pensum2014, new Curso(8, "ALGORITMOS", sistemas)),
                new PensumCurso(2, 5, pensum2014, new Curso(9, "CONTABILIDAD II", sistemas)),
                new PensumCurso(2, 5, pensum2014, new Curso(10, "MATEMATICA DISCRETA", sistemas)),

                new PensumCurso(3, 5, pensum2014, new Curso(11, "FISICA I", sistemas)),
                new PensumCurso(3, 5, pensum2014, new Curso(12, "PROGRAMACION I", sistemas)),
                new PensumCurso(3, 5, pensum2014, new Curso(13, "CALCULO I", sistemas)),
                new PensumCurso(3, 4, pensum2014, new Curso(14, "PROCESO ADMINISTRATIVO", sistemas)),
                new PensumCurso(3, 5, pensum2014, new Curso(15, "DERECHO INFORMATICO", sistemas)),

                new PensumCurso(4, 5, pensum2014, new Curso(16, "MICROECONOMIA", sistemas)),
                new PensumCurso(4, 5, pensum2014, new Curso(17, "PROGRAMACION II", sistemas)),
                new PensumCurso(4, 5, pensum2014, new Curso(18, "CALCULO II", sistemas)),
                new PensumCurso(4, 5, pensum2014, new Curso(19, "ESTADISTICA I", sistemas)),
                new PensumCurso(4, 5, pensum2014, new Curso(20, "FISICA II", sistemas)),

                new PensumCurso(5, 5, pensum2014, new Curso(21, "METODOS NUMERICOS", sistemas)),
                new PensumCurso(5, 5, pensum2014, new Curso(22, "PROGRAMACION III", sistemas)),
                new PensumCurso(5, 5, pensum2014, new Curso(23, "EMPRENDEDORES DE NEGOCIOS", sistemas)),
                new PensumCurso(5, 5, pensum2014, new Curso(24, "ELECTRONICA ANALOGICA", sistemas)),
                new PensumCurso(5, 5, pensum2014, new Curso(25, "ESTADISTICA II", sistemas)),

                new PensumCurso(6, 5, pensum2014, new Curso(26, "INVESTIGACION DE OPERACIONES", sistemas)),
                new PensumCurso(6, 5, pensum2014, new Curso(27, "BASES DE DATOS I", sistemas)),
                new PensumCurso(6, 5, pensum2014, new Curso(28, "AUTOMATAS Y LENGUAJES FORMALES", sistemas)),
                new PensumCurso(6, 5, pensum2014, new Curso(29, "SISTEMAS OPERATIVOS I", sistemas)),
                new PensumCurso(6, 5, pensum2014, new Curso(30, "ELECTRONICA DIGITAL", sistemas)),

                new PensumCurso(7, 5, pensum2014, new Curso(31, "BASES DE DATOS II", sistemas)),
                new PensumCurso(7, 5, pensum2014, new Curso(32, "ANALISIS DE SISTEMAS I", sistemas)),
                new PensumCurso(7, 5, pensum2014, new Curso(33, "SISTEMAS OPERATIVOS II", sistemas)),
                new PensumCurso(7, 5, pensum2014, new Curso(34, "ARQUITECTURA DE COMPUTADORAS I", sistemas)),
                new PensumCurso(7, 5, pensum2014, new Curso(35, "COMPILADORES", sistemas)),

                new PensumCurso(8, 5, pensum2014, new Curso(36, "DESARROLLO WEB", sistemas)),
                new PensumCurso(8, 5, pensum2014, new Curso(37, "ANALISIS DE SISTEMAS II", sistemas)),
                new PensumCurso(8, 5, pensum2014, new Curso(38, "REDES DE COMPUTADORAS I", sistemas)),
                new PensumCurso(8, 4, pensum2014, new Curso(39, "ETICA PROFESIONAL", sistemas)),
                new PensumCurso(8, 5, pensum2014, new Curso(40, "ARQUITECTURA DE COMPUTADORAS II", sistemas)),

                new PensumCurso(9, 5, pensum2014, new Curso(41, "ADMINISTRACION DE TECNOLOGIAS DE INFORMACION", sistemas)),
                new PensumCurso(9, 5, pensum2014, new Curso(42, "INGENIERIA DE SOFTWARE", sistemas)),
                new PensumCurso(9, 6, pensum2014, new Curso(43, "PROYECTO DE GRADUACION I", sistemas)),
                new PensumCurso(9, 5, pensum2014, new Curso(44, "REDES DE COMPUTADORAS II", sistemas)),
                new PensumCurso(9, 5, pensum2014, new Curso(45, "INTELIGENCIA ARTIFICIAL", sistemas)),

                new PensumCurso(10, 5, pensum2014, new Curso(46, "TELECOMUNICACIONES", sistemas)),
                new PensumCurso(10, 6, pensum2014, new Curso(47, "SEMINARIOS DE TECNOLOGIAS DE INFORMACION", sistemas)),
                new PensumCurso(10, 5, pensum2014, new Curso(48, "ASEGURAMIENTO DE LA CALIDAD DEL SOFTWARE", sistemas)),
                new PensumCurso(10, 6, pensum2014, new Curso(49, "PROYECTO DE GRADUACION II", sistemas)),
                new PensumCurso(10, 5, pensum2014, new Curso(50, "SEGURIDAD Y AUDITORIA DE SISTEMAS", sistemas)),

            };

            foreach(var pc in pensumCursos)
            {
                db.Cursos.Add(pc.Curso);
                db.PensumCursos.Add(pc);
            }

            await db.SaveChangesAsync();

        }

    }
}
