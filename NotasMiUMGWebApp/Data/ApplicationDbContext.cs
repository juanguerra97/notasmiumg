using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using jguerra.notasmiumg;

namespace NotasMiUMGWebApp.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<IdentityUser>
    {

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Pensum> Pensums { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<PensumCurso> PensumCursos { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Nota> Notas { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //###### Entidad Carrera ###########################################################################
            modelBuilder.Entity<Carrera>(carrera =>
            {
                carrera.HasKey(c => c.CodigoCarrera); // primary key
                carrera.HasAlternateKey(c => c.NombreCarrera); // el nombre de la carrera debe ser unico
                carrera.Property(c => c.NombreCarrera).HasMaxLength(64);
                carrera.HasMany(c => c.Cursos) // una carrera tiene muchos cursos
                    .WithOne(curso => curso.Carrera) // un curso pertenece a una carrera
                    .HasForeignKey(curso => curso.CodigoCarrera) //foreign key
                    .OnDelete(DeleteBehavior.Restrict);
                carrera.HasMany(c => c.Pensums) // una carrera tiene muchos pensums
                    .WithOne(pensum => pensum.Carrera) // un pensum pertenece a una carrera
                    .HasForeignKey(pensum => pensum.CodigoCarrera) //foreign key
                    .OnDelete(DeleteBehavior.Restrict);
            });
            //modelBuilder.Entity<Carrera>()
            //    .HasKey(c => c.CodigoCarrera); // primary key
            //modelBuilder.Entity<Carrera>()
            //    .HasAlternateKey(c => c.NombreCarrera); // el nombre de la carrera debe ser unico
            //modelBuilder.Entity<Carrera>()
            //    .Property(c => c.NombreCarrera)
            //    .HasMaxLength(64);
            //modelBuilder.Entity<Carrera>()
            //    .HasMany(carrera => carrera.Cursos) // una carrera tiene muchos cursos
            //    .WithOne(curso => curso.Carrera) // un curso pertenece a una carrera
            //    .HasForeignKey(curso => curso.CodigoCarrera) //foreign key
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Carrera>()
            //    .HasMany(carrera => carrera.Pensums) // una carrera tiene muchos pensums
            //    .WithOne(pensum => pensum.Carrera) // un pensum pertenece a una carrera
            //    .HasForeignKey(pensum => pensum.CodigoCarrera) //foreign key
            //    .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad Curso #############################################################################
            modelBuilder.Entity<Curso>(curso =>
            {
                curso.HasKey(c => new { c.CodigoCarrera, c.CodigoCurso }); // primary key
                curso.HasAlternateKey(c => new { c.CodigoCarrera, c.NombreCurso });// el nombre de un curso debe ser unico dentro de la misma carrera
                curso.Property(c => c.NombreCurso).HasMaxLength(64);
                curso.HasOne(c => c.Carrera) // un curso pertenece a una carrera
                    .WithMany(carrera => carrera.Cursos) // una carrera tiene muchos cursos
                    .HasForeignKey(cur => cur.CodigoCarrera) // foreign key
                    .OnDelete(DeleteBehavior.Restrict);
                curso.HasMany(c => c.PensumCursos) // un curso puede pertenecer a varios pensum
                    .WithOne(pensumCurso => pensumCurso.Curso) // cada pensum puede tener varios cursos
                    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.CodigoCurso }) // foreign key
                    .OnDelete(DeleteBehavior.Restrict);
            });
            //modelBuilder.Entity<Curso>()
            //    .HasKey(c => new { c.CodigoCarrera, c.CodigoCurso }); // primary key
            //modelBuilder.Entity<Curso>()
            //    .HasAlternateKey(c => new { c.CodigoCarrera, c.NombreCurso });// el nombre de un curso debe ser unico dentro de la misma carrera
            //modelBuilder.Entity<Curso>()
            //    .Property(curso => curso.NombreCurso)
            //    .HasMaxLength(64);
            //modelBuilder.Entity<Curso>()
            //    .HasOne(curso => curso.Carrera) // un curso pertenece a una carrera
            //    .WithMany(carrera => carrera.Cursos) // una carrera tiene muchos cursos
            //    .HasForeignKey(curso => curso.CodigoCarrera) // foreign key
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Curso>()
            //    .HasMany(curso => curso.PensumCursos) // un curso puede pertenecer a varios pensum
            //    .WithOne(pensumCurso => pensumCurso.Curso) // cada pensum puede tener varios cursos
            //    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.CodigoCurso }) // foreign key
            //    .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad Pensum ############################################################################
            modelBuilder.Entity<Pensum>(pensum =>
            {
                pensum.HasKey(p => new { p.CodigoCarrera, p.AnoPensum });// primary key
                pensum.HasMany(p => p.Estudiantes) // en un pensum de cierta carrera hay muchos estudiantes
                    .WithOne(estudiante => estudiante.Pensum) // un estudiante pertenece a un pensum de cierta carrera
                    .OnDelete(DeleteBehavior.Restrict);
                pensum.HasOne(pensum => pensum.Carrera) // un pensum pertenece a una carrera
                    .WithMany(carrera => carrera.Pensums) // una carrera puede tener varios pensum
                    .HasForeignKey(pensum => pensum.CodigoCarrera) // foreign key
                    .OnDelete(DeleteBehavior.Restrict);
                pensum.HasMany(pensum => pensum.PensumCursos) // en un pensum puede haber varios cursos
                    .WithOne(pensumCurso => pensumCurso.Pensum) // un curso puede estar en varios pensum
                    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.AnoPensum }) // foreign key
                    .OnDelete(DeleteBehavior.Restrict);

            });
            //modelBuilder.Entity<Pensum>()
            //    .HasKey(p => new { p.CodigoCarrera, p.AnoPensum });// primary key
            //modelBuilder.Entity<Pensum>()
            //    .HasMany(pensum => pensum.Estudiantes) // en un pensum de cierta carrera hay muchos estudiantes
            //    .WithOne(estudiante => estudiante.Pensum) // un estudiante pertenece a un pensum de cierta carrera
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Pensum>()
            //    .HasOne(pensum => pensum.Carrera) // un pensum pertenece a una carrera
            //    .WithMany(carrera => carrera.Pensums) // una carrera puede tener varios pensum
            //    .HasForeignKey(pensum => pensum.CodigoCarrera) // foreign key
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Pensum>()
            //    .HasMany(pensum => pensum.PensumCursos) // en un pensum puede haber varios cursos
            //    .WithOne(pensumCurso => pensumCurso.Pensum) // un curso puede estar en varios pensum
            //    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.AnoPensum }) // foreign key
            //    .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad PensumCurso #########################################################################
            modelBuilder.Entity<PensumCurso>(pensumCurso => 
            { 
                pensumCurso.HasKey(p => new { p.CodigoCarrera, p.AnoPensum, p.CodigoCurso }); // primary key
                pensumCurso.HasOne(pc => pc.Pensum)
                    .WithMany(pensum => pensum.PensumCursos)
                    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.AnoPensum })
                    .OnDelete(DeleteBehavior.Restrict);
                pensumCurso.HasOne(pc => pc.Curso)
                    .WithMany(curso => curso.PensumCursos)
                    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.CodigoCurso })
                    .OnDelete(DeleteBehavior.Restrict);
                pensumCurso.HasMany(pc => pc.Notas) // cada combinacion pensum-curso tiene varias notas de los estudiantes
                    .WithOne(nota => nota.PensumCurso) // cada nota de un estudiante es de un curso de cierto pensum
                    .OnDelete(DeleteBehavior.Restrict);

            });
            //modelBuilder.Entity<PensumCurso>()
            //    .HasKey(p => new { p.CodigoCarrera, p.AnoPensum, p.CodigoCurso }); // primary key
            //modelBuilder.Entity<PensumCurso>()
            //    .HasOne(pc => pc.Pensum)
            //    .WithMany(pensum => pensum.PensumCursos)
            //    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.AnoPensum })
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<PensumCurso>()
            //    .HasOne(pc => pc.Curso)
            //    .WithMany(curso => curso.PensumCursos)
            //    .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.CodigoCurso })
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<PensumCurso>()
            //    .HasMany(pc => pc.Notas) // cada combinacion pensum-curso tiene varias notas de los estudiantes
            //    .WithOne(nota => nota.PensumCurso) // cada nota de un estudiante es de un curso de cierto pensum
            //    .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad Estudiante #########################################################################
            modelBuilder.Entity<Estudiante>(estudiante =>
            {
                estudiante.HasKey(e => e.EstudianteId); // primary key
                estudiante.HasAlternateKey(e => e.Carne); // el carne debe ser unico
                estudiante.Property(e => e.EstudianteId).ValueGeneratedOnAdd(); // el id debe ser generado automaticamente
                estudiante.Property(e => e.Carne).HasMaxLength(16);
                estudiante.Property(e => e.Nombre).IsRequired().HasMaxLength(64);
                estudiante.Property(e => e.Apellido).IsRequired().HasMaxLength(64);
                estudiante.HasOne(e => e.Pensum) // un estudiante pertenece a un pensum de una carrera
                    .WithMany(pensum => pensum.Estudiantes) // en un pensum de una carrera pueden haber varios estudiantes
                    .OnDelete(DeleteBehavior.Restrict);
                estudiante.HasMany(e => e.Notas) // un estudiante tiene varias notas
                    .WithOne(nota => nota.Estudiante) // una nota pertenece a un estudiante
                    .HasForeignKey(nota => nota.EstudianteId) // foreign key
                    .OnDelete(DeleteBehavior.Restrict);
                estudiante.HasOne(e => e.UsuarioEstudiante); // un estudiante tiene un usuario con el cual ingresa a la aplicacion

            });
            //modelBuilder.Entity<Estudiante>().HasKey(e => e.EstudianteId); // primary key
            //modelBuilder.Entity<Estudiante>().HasAlternateKey(e => e.Carne); // el carne debe ser unico
            //modelBuilder.Entity<Estudiante>()
            //    .Property(e => e.EstudianteId)
            //    .ValueGeneratedOnAdd(); // el id debe ser generado automaticamente
            //modelBuilder.Entity<Estudiante>()
            //    .Property(e => e.Carne)
            //    .HasMaxLength(16);
            //modelBuilder.Entity<Estudiante>()
            //    .Property(e => e.Nombre)
            //    .IsRequired()
            //    .HasMaxLength(64);
            //modelBuilder.Entity<Estudiante>()
            //    .Property(e => e.Apellido)
            //    .IsRequired()
            //    .HasMaxLength(64);
            //modelBuilder.Entity<Estudiante>()
            //    .HasOne(e => e.Pensum) // un estudiante pertenece a un pensum de una carrera
            //    .WithMany(pensum => pensum.Estudiantes) // en un pensum de una carrera pueden haber varios estudiantes
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Estudiante>()
            //    .HasMany(e => e.Notas) // un estudiante tiene varias notas
            //    .WithOne(nota => nota.Estudiante) // una nota pertenece a un estudiante
            //    .HasForeignKey(nota => nota.EstudianteId) // foreign key
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Estudiante>()
            //    .HasOne(e => e.UsuarioEstudiante);

            //###### Entidad Nota ###########################################################################
            modelBuilder.Entity<Nota>(nota => 
            { 
                nota.HasKey(n => new
                {
                    n.EstudianteId,
                    n.CodigoCarrera,
                    n.AnoPensum,
                    n.CodigoCurso,
                    n.Ano // primary key
                });
                nota.Property(nota => nota.Zona) // la zona se calcula a partir del primer y segundo parcial y las actividades
                    .HasComputedColumnSql("PrimerParcial + SegundoParcial + Actividades"); 
                nota.Property(nota => nota.NotaFinal) // la nota final es la suma de la zona mas la calificacion en el examen final
                    .HasComputedColumnSql("Zona + ExamenFinal"); 
                nota.Property(nota => nota.Aprobado).HasComputedColumnSql("NotaFinal >= 61"); // se aprueba con 61 puntos
                nota.HasOne(nota => nota.Estudiante) // una nota pertenece a un estudiante
                    .WithMany(estudiante => estudiante.Notas) // un estudiante tiene varias notas
                    .HasForeignKey(nota => nota.EstudianteId) // foreign key
                    .OnDelete(DeleteBehavior.Restrict);
                nota.HasOne(nota => nota.PensumCurso) // una nota es de un curso en cierto pensum
                    .WithMany(pensumCurso => pensumCurso.Notas) // un curso en cierto pensum tiene muchas notas
                    .HasForeignKey(nota => new
                    {
                        nota.CodigoCarrera,
                        nota.AnoPensum,
                        nota.CodigoCurso // foreign key
                    })
                    .OnDelete(DeleteBehavior.Restrict); ;

            });
            //modelBuilder.Entity<Nota>()
            //    .HasKey(n => new
            //    {
            //        n.EstudianteId,
            //        n.CodigoCarrera,
            //        n.AnoPensum,
            //        n.CodigoCurso,
            //        n.Ano // primary key
            //    });
            //modelBuilder.Entity<Nota>()
            //    .Property(nota => nota.Zona)
            //    .HasComputedColumnSql("PrimerParcial + SegundoParcial + Actividades"); // la zona se calcula a partir del primer y segundo parcial y las actividades
            //modelBuilder.Entity<Nota>()
            //    .Property(nota => nota.NotaFinal)
            //    .HasComputedColumnSql("Zona + ExamenFinal"); // la nota final es la suma de la zona mas la calificacion en el examen final
            //modelBuilder.Entity<Nota>()
            //    .Property(nota => nota.Aprobado)
            //    .HasComputedColumnSql("NotaFinal >= 61"); // se aprueba con 61 puntos
            //modelBuilder.Entity<Nota>()
            //    .HasOne(nota => nota.Estudiante) // una nota pertenece a un estudiante
            //    .WithMany(estudiante => estudiante.Notas) // un estudiante tiene varias notas
            //    .HasForeignKey(nota => nota.EstudianteId) // foreign key
            //    .OnDelete(DeleteBehavior.Restrict); 
            //modelBuilder.Entity<Nota>()
            //    .HasOne(nota => nota.PensumCurso) // una nota es de un curso en cierto pensum
            //    .WithMany(pensumCurso => pensumCurso.Notas) // un curso en cierto pensum tiene muchas notas
            //    .HasForeignKey(nota => new
            //    {
            //        nota.CodigoCarrera,
            //        nota.AnoPensum,
            //        nota.CodigoCurso // foreign key
            //    })
            //    .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
