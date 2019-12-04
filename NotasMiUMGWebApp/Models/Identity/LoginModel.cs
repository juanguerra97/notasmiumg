using System.ComponentModel.DataAnnotations;

namespace NotasMiUMGWebApp.Models.Identity
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Falta nombre de usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Falta contraseña")]
        public string Password { get; set; }
    }

}
