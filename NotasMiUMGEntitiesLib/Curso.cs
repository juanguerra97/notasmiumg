using System;
using System.Collections.Generic;

namespace jguerra.notasmiumg
{
    /// <summary>
    /// Clase para representar los cursos.
    /// Cada curso pertenece a una carrera y tiene un codigo que debe ser unico dentro de esa carrera.
    /// El nombre de un curso tambien debe ser unico dentro de la misma carrera.
    /// Un curso puede pertenecer a varios pensum de la misma carrera.
    /// </summary>
    public class Curso
    {

        public uint CodigoCurso { get; set; }

        public string NombreCurso { get; set; }

        public uint CodigoCarrera { get; set; }

        public Carrera Carrera { get; set; }

        public List<PensumCurso> PensumCursos { get; set; }

        public Curso()
            :this(default,default,default)
        {
            PensumCursos = new List<PensumCurso>();
        }

        public Curso(uint codigoCurso, string nombreCurso, Carrera carrera)
        {
            CodigoCurso = codigoCurso;
            NombreCurso = nombreCurso;
            Carrera = carrera;
        }

        public override bool Equals(object obj)
        {
            return obj is Curso curso &&
                   CodigoCurso == curso.CodigoCurso &&
                   EqualityComparer<Carrera>.Default.Equals(Carrera, curso.Carrera);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CodigoCurso, Carrera);
        }

        public override string ToString()
        {
            return $"{CodigoCurso} {NombreCurso}";
        }

    }
}
