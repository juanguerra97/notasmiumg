using System;
using System.Collections.Generic;

namespace jguerra.notasmiumg
{

    /// <summary>
    /// Clase que representa los pensum de una carrera. El identificador de un pensum es su año 
    /// el cual debe ser unico dentro de la misma carrera.
    /// Un pensum tiene varios cursos que se llevan en este pensum.
    /// </summary>
    public class Pensum
    {

        public uint AnoPensum { get; set; }

        public uint CodigoCarrera { get; set; }
        public Carrera Carrera { get; set; }

        public List<Estudiante> Estudiantes { get; set; }

        public List<PensumCurso> PensumCursos { get; set; }

        public Pensum()
            :this(default,default)
        {
            PensumCursos = new List<PensumCurso>();
            Estudiantes = new List<Estudiante>();
        }

        public Pensum(uint anoPensum, Carrera carrera)
        {
            AnoPensum = anoPensum;
            Carrera = carrera;
        }

        public override bool Equals(object obj)
        {
            return obj is Pensum pensum &&
                   AnoPensum == pensum.AnoPensum &&
                   EqualityComparer<Carrera>.Default.Equals(Carrera, pensum.Carrera);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AnoPensum, Carrera);
        }

        public override string ToString()
        {
            return $"{AnoPensum}, {Carrera.ToString()}";
        }

    }
}
