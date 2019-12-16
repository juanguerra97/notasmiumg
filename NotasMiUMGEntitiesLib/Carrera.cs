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

        public string NombreCarrera { get; set; }

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
