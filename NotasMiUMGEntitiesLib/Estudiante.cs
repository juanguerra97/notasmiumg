using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace jguerra.notasmiumg
{
    public class Estudiante
    {

        // Expresion regular para validar un carne de estudiante
        private readonly Regex REGEX_CARNE = new Regex(@"^\d{4}-\d{2}-\d{1,8}$", RegexOptions.Compiled);

        public uint EstudianteId { get; set; }

        public string Carne { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public uint AnoInicio { get; set; }

        public Pensum Pensum { get; set; }

        public List<Nota> Notas { get; set; }

        public IdentityUser UsuarioEstudiante { get; set; }

        public Estudiante()
            :this(default,default,default,default,default,default)
        {
            Notas = new List<Nota>();
        }

        public Estudiante(string carne, string nombre, string apellido, uint anoInicio, Pensum pensum, IdentityUser usuarioEstudiante)
        {
            Carne = carne;
            Nombre = nombre;
            Apellido = apellido;
            AnoInicio = anoInicio;
            Pensum = pensum;
            UsuarioEstudiante = usuarioEstudiante;
        }

        public override bool Equals(object obj)
        {
            return obj is Estudiante estudiante &&
                   EstudianteId == estudiante.EstudianteId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EstudianteId);
        }

        public override string ToString() => $"{Carne} {Nombre} {Apellido}";

    }
}
