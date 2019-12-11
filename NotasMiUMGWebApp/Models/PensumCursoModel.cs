using System.ComponentModel.DataAnnotations;

namespace NotasMiUMGWebApp.Models
{
    public class PensumCursoModel
    {

        [Required(ErrorMessage = "Falta el código de carrera")]
        public uint CodigoCarrera { get; set; }

        [Required(ErrorMessage = "Falta el año del pensum")]
        public uint AnoPensum { get; set; }

        [Required(ErrorMessage = "Falta el código del curso")]
        public uint CodigoCurso { get; set; }

        [Required(ErrorMessage = "Falta el ciclo")]
        public byte Ciclo { get; set; }

        [Required(ErrorMessage = "Faltan los créditos")]
        public byte Creditos { get; set; }


    }
}
