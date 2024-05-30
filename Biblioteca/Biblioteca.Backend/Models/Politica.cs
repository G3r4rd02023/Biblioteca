using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Backend.Models
{
    public class Politica
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Valor { get; set; }
    }
}
