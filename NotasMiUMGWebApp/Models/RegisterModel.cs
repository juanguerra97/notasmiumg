using System.ComponentModel.DataAnnotations;

namespace NotasMiUMGWebApp.Models
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Falta nombre de usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Falta contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Falta el carné")]
        public string Carne { get; set; }

        [Required(ErrorMessage = "Falta el nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Falta el apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Falta el año de inicio")]
        public uint AnoInicio { get; set; }

        [Required(ErrorMessage = "Falta el código de la carrera")]
        public uint CodigoCarrera { get; set; }

        [Required(ErrorMessage = "Falta el año del pensum")]
        public uint AnoPensum { get; set; }

    }
}
