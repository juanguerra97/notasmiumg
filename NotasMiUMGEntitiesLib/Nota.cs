using System;
using System.Collections.Generic;

namespace jguerra.notasmiumg
{
    
    /// <summary>
    /// Clase para representar las notas de los estudiantes en los diferentes cursos de la carrera.
    /// Una nota esta formada por la calificacion del primer y segundo parcial, actividades(tareas) 
    /// y examen final.
    /// La zona es la suma de las calificaciones de los dos parciales mas las actividades.
    /// </summary>
    public class Nota
    {
     
        public uint Ano { get; set; }

        public byte PrimerParcial { get; set; }

        public byte SegundoParcial { get; set; }

        public byte Actividades { get; set; }

        public byte Zona { get; set; }

        public byte ExamenFinal { get; set; }

        public byte NotaFinal { get; set; }

        public bool Aprobado { get; set; }

        public uint EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
        
        public uint CodigoCarrera { get; set; }
        public uint AnoPensum { get; set; }
        public uint CodigoCurso { get; set; }
        public PensumCurso PensumCurso { get; set; }

        public Nota()
            :this(default,default,default,default,default,default,default)
        {
        }

        public Nota(uint ano, byte primerParcial, byte segundoParcial, byte actividades, byte examenFinal, Estudiante estudiante, PensumCurso pensumCurso)
        {
            Ano = ano;
            PrimerParcial = primerParcial;
            SegundoParcial = segundoParcial;
            Actividades = actividades;
            ExamenFinal = examenFinal;
            Estudiante = estudiante;
            PensumCurso = pensumCurso;
        }

        public override bool Equals(object obj)
        {
            return obj is Nota nota &&
                   Ano == nota.Ano &&
                   EqualityComparer<Estudiante>.Default.Equals(Estudiante, nota.Estudiante) &&
                   EqualityComparer<PensumCurso>.Default.Equals(PensumCurso, nota.PensumCurso);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ano, Estudiante, PensumCurso);
        }

        public override string ToString() => $"{Ano} {Estudiante.ToString()}, {PensumCurso.ToString()}, {NotaFinal}";

    }
}
