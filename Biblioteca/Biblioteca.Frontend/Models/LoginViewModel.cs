using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Frontend.Models
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(100)]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Clave { get; set; }

        
    }
}
