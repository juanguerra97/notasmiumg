using System.ComponentModel.DataAnnotations;

namespace NotasMiUMGWebApp.Models
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Falta nombre de usuario")]
        public string username { get; set; }

        [Required(ErrorMessage = "Falta contraseña")]
        public string password { get; set; }

        [Required(ErrorMessage = "Falta el carné")]
        public string carne { get; set; }

        [Required(ErrorMessage = "Falta el nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Falta el apellido")]
        public string apellido { get; set; }

        [Required(ErrorMessage = "Falta el año de inicio")]
        public uint anoInicio { get; set; }

        [Required(ErrorMessage = "Falta el código de la carrera")]
        public uint codigoCarrera { get; set; }

        [Required(ErrorMessage = "Falta el año del pensum")]
        public uint anoPensum { get; set; }

    }
}
