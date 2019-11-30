using System;
using System.Collections.Generic;

namespace jguerra.notasmiumg
{ 

    /// <summary>
    /// Clase para representar las carreras, toda carrera tiene un codigo 
    /// el cual es un numero entero y un nombre que debe ser unico
    /// </summary>
    public class Carrera
    {

        public uint CodigoCarrera { get; set; }

        private string _nombreCarrera;
        public string NombreCarrera {
            get => _nombreCarrera; 
            set
            {
                value = value?.Trim(); // si la cadena no es null, se eliminan los caracteres de espacio al inicio y al final
                if(string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("El nombre no puede estar vacío");
                }
                _nombreCarrera = value.ToUpper();
            }
        }

        public List<Curso> Cursos { get; set; }

        public List<Pensum> Pensums { get; set; }

        public Carrera()
            :this(default,default)
        {
            Cursos = new List<Curso>();
            Pensums = new List<Pensum>();
        }

        public Carrera(uint codigoCarrera, string nombreCarrera)
        {
            CodigoCarrera = codigoCarrera;
            NombreCarrera = nombreCarrera;
        }

        public override bool Equals(object obj)
        {
            return obj is Carrera carrera &&
                   CodigoCarrera == carrera.CodigoCarrera;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CodigoCarrera);
        }

        public override string ToString()
        {
            return $"{CodigoCarrera} {NombreCarrera}";
        }

    }
}
