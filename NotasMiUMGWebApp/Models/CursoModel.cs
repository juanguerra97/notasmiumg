using System.ComponentModel.DataAnnotations;

namespace NotasMiUMGWebApp.Models
{
    public class CursoModel
    {

        [Required(ErrorMessage = "Falta el código de la carrera")]
        public uint CodigoCarrera { get; set; }

        [Required(ErrorMessage = "Falta el código del curso")]
        public uint CodigoCurso { get; set; }

        [Required(ErrorMessage = "Falta el nombre del curso")]
        public string NombreCurso { get; set; }

    }
}
