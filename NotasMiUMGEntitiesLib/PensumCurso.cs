using System;
using System.Collections.Generic;

namespace jguerra.notasmiumg
{

    /// <summary>
    /// Clase para relacionar pensums y cursos.
    /// Se debe establecer el ciclo en el que se lleva el curso 
    /// para este pensum y los creditos que se obtienen al aprobarlo.
    /// Para cada relacion pensum-curso hay un conjunto de notas de todos 
    /// los estudiantes que han tomado este curso en este pensum.
    /// </summary>
    public class PensumCurso
    {

        public byte Ciclo { get; set; }

        public byte Creditos { get; set; }

        public uint CodigoCarrera { get; set; }
        public uint AnoPensum { get; set; }
        public Pensum Pensum { get; set; }

        public uint CodigoCurso { get; set; }
        public Curso Curso { get; set; }

        public List<Nota> Notas { get; set; }

        public PensumCurso()
            :this(default,default,default,default)
        {
            Notas = new List<Nota>();
        }

        public PensumCurso(byte ciclo, byte creditos, Pensum pensum, Curso curso)
        {
            Ciclo = ciclo;
            Creditos = creditos;
            Pensum = pensum;
            Curso = curso;
        }


        public override bool Equals(object obj)
        {
            return obj is PensumCurso curso &&
                   EqualityComparer<Pensum>.Default.Equals(Pensum, curso.Pensum) &&
                   EqualityComparer<Curso>.Default.Equals(Curso, curso.Curso);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Pensum, Curso);
        }

        public override string ToString() => $"{Pensum.ToString()} {Curso.ToString()}";

    }
}
