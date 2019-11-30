using System;
using Microsoft.EntityFrameworkCore;
using jguerra.notasmiumg;

namespace jguerra.db.context
{

    /// <summary>
    /// Clase con el contexto de la base de datos para Entity Framework Core
    /// </summary>
    public class NotasMiUmg : DbContext
    {

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Pensum> Pensums { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<PensumCurso> PensumCursos { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Nota> Notas { get; set; }

        public NotasMiUmg(DbContextOptions<NotasMiUmg> options)
            :base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //###### Entidad Carrera ###########################################################################
            modelBuilder.Entity<Carrera>().HasKey(c => c.CodigoCarrera); // primary key
            modelBuilder.Entity<Carrera>()
                .HasAlternateKey(c => c.NombreCarrera); // el nombre de la carrera debe ser unico
            modelBuilder.Entity<Carrera>()
                .Property(c => c.NombreCarrera)
                .HasMaxLength(64);
            modelBuilder.Entity<Carrera>()
                .HasMany(carrera => carrera.Cursos) // una carrera tiene muchos cursos
                .WithOne(curso => curso.Carrera) // un curso pertenece a una carrera
                .HasForeignKey(curso => curso.CodigoCarrera) //foreign key
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Carrera>()
                .HasMany(carrera => carrera.Pensums) // una carrera tiene muchos pensums
                .WithOne(pensum => pensum.Carrera) // un pensum pertenece a una carrera
                .HasForeignKey(pensum => pensum.CodigoCarrera) //foreign key
                .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad Curso #############################################################################
            modelBuilder.Entity<Curso>()
                .HasKey(c => new { c.CodigoCarrera, c.CodigoCurso}); // primary key
            modelBuilder.Entity<Curso>()
                .HasAlternateKey(c => new { c.CodigoCarrera, c.NombreCurso });// el nombre de un curso debe ser unico dentro de la misma carrera
            modelBuilder.Entity<Curso>()
                .Property(curso => curso.NombreCurso)
                .HasMaxLength(64);
            modelBuilder.Entity<Curso>()
                .HasOne(curso => curso.Carrera) // un curso pertenece a una carrera
                .WithMany(carrera => carrera.Cursos) // una carrera tiene muchos cursos
                .HasForeignKey(curso => curso.CodigoCarrera) // foreign key
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Curso>()
                .HasMany(curso => curso.PensumCursos) // un curso puede pertenecer a varios pensum
                .WithOne(pensumCurso => pensumCurso.Curso) // cada pensum puede tener varios cursos
                .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.CodigoCurso }) // foreign key
                .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad Pensum ############################################################################
            modelBuilder.Entity<Pensum>()
                .HasKey(p => new { p.CodigoCarrera, p.AnoPensum });// primary key
            modelBuilder.Entity<Pensum>()
                .HasMany(pensum => pensum.Estudiantes) // en un pensum de cierta carrera hay muchos estudiantes
                .WithOne(estudiante => estudiante.Pensum) // un estudiante pertenece a un pensum de cierta carrera
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Pensum>(p =>
            {
            });
            modelBuilder.Entity<Pensum>()
                .HasOne(pensum => pensum.Carrera) // un pensum pertenece a una carrera
                .WithMany(carrera => carrera.Pensums) // una carrera puede tener varios pensum
                .HasForeignKey(pensum => pensum.CodigoCarrera) // foreign key
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Pensum>()
                .HasMany(pensum => pensum.PensumCursos) // en un pensum puede haber varios cursos
                .WithOne(pensumCurso => pensumCurso.Pensum) // un curso puede estar en varios pensum
                .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.AnoPensum }) // foreign key
                .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad PensumCurso #########################################################################
            modelBuilder.Entity<PensumCurso>()
                .HasKey(p => new { p.CodigoCarrera, p.AnoPensum, p.CodigoCurso }); // primary key
            modelBuilder.Entity<PensumCurso>()
                .HasOne(pc => pc.Pensum)
                .WithMany(pensum => pensum.PensumCursos)
                .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.AnoPensum })
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PensumCurso>()
                .HasOne(pc => pc.Curso)
                .WithMany(curso => curso.PensumCursos)
                .HasForeignKey(pensumCurso => new { pensumCurso.CodigoCarrera, pensumCurso.CodigoCurso })
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PensumCurso>()
                .HasMany(pc => pc.Notas) // cada combinacion pensum-curso tiene varias notas de los estudiantes
                .WithOne(nota => nota.PensumCurso) // cada nota de un estudiante es de un curso de cierto pensum
                .OnDelete(DeleteBehavior.Restrict);


            //###### Entidad Estudiante #########################################################################
            modelBuilder.Entity<Estudiante>().HasKey(e => e.EstudianteId); // primary key
            modelBuilder.Entity<Estudiante>().HasAlternateKey(e => e.Carne); // el carne debe ser unico
            modelBuilder.Entity<Estudiante>()
                .Property(e => e.EstudianteId)
                .ValueGeneratedOnAdd(); // el id debe ser generado automaticamente
            modelBuilder.Entity<Estudiante>()
                .Property(e => e.Carne)
                .HasMaxLength(16);
            modelBuilder.Entity<Estudiante>()
                .Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(64);
            modelBuilder.Entity<Estudiante>()
                .Property(e => e.Apellido)
                .IsRequired()
                .HasMaxLength(64);
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.Pensum) // un estudiante pertenece a un pensum de una carrera
                .WithMany(pensum => pensum.Estudiantes) // en un pensum de una carrera pueden haber varios estudiantes
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Estudiante>()
                .HasMany(e => e.Notas) // un estudiante tiene varias notas
                .WithOne(nota => nota.Estudiante) // una nota pertenece a un estudiante
                .HasForeignKey(nota => nota.EstudianteId) // foreign key
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.UsuarioEstudiante)
                .WithOne();

            //###### Entidad Nota ###########################################################################
            modelBuilder.Entity<Nota>()
                .HasKey(n => new  
                { 
                    n.EstudianteId, n.CodigoCarrera, n.AnoPensum, n.CodigoCurso , n.Ano // primary key
                });
            modelBuilder.Entity<Nota>()
                .Property(nota => nota.Zona)
                .HasComputedColumnSql("PrimerParcial + SegundoParcial + Actividades"); // la zona se calcula a partir del primer y segundo parcial y las actividades
            modelBuilder.Entity<Nota>()
                .Property(nota => nota.NotaFinal)
                .HasComputedColumnSql("Zona + ExamenFinal"); // la nota final es la suma de la zona mas la calificacion en el examen final
            modelBuilder.Entity<Nota>()
                .Property(nota => nota.Aprobado)
                .HasComputedColumnSql("NotaFinal >= 61"); // se aprueba con 61 puntos
            modelBuilder.Entity<Nota>()
                .HasOne(nota => nota.Estudiante) // una nota pertenece a un estudiante
                .WithMany(estudiante => estudiante.Notas) // un estudiante tiene varias notas
                .HasForeignKey(nota => nota.EstudianteId) // foreign key
                .OnDelete(DeleteBehavior.Restrict); ;
            modelBuilder.Entity<Nota>()
                .HasOne(nota => nota.PensumCurso) // una nota es de un curso en cierto pensum
                .WithMany(pensumCurso => pensumCurso.Notas) // un curso en cierto pensum tiene muchas notas
                .HasForeignKey(nota => new 
                    { 
                        nota.CodigoCarrera, nota.AnoPensum, nota.CodigoCurso // foreign key
                }) 
                .OnDelete(DeleteBehavior.Restrict); ;

        }
    }
}
