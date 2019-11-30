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

        private string _carne;
        public string Carne {
            get => _carne;  
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("El carne no puede estar vacio");
                }
                if(!REGEX_CARNE.IsMatch(value))
                {
                    throw new ArgumentException("El formato del carne es invalido");
                }
                _carne = value;
            }
        }

        private string _nombre;
        public string Nombre {
            get => _nombre;
            set
            {
                value = value?.Trim();
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("El nombre no puede estar vacio");
                }
                _nombre = value;
            }
        }

        private string _apellido;
        public string Apellido {
            get => _apellido; 
            set {
                value = value?.Trim();
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("El apellido no puede estar vacio");
                }
                _apellido = value;
            }
        }

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
