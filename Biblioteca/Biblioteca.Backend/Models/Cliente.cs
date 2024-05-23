using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Backend.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Cliente")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

    }
}
