using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Backend.Models
{
    public class Correlativo
    {
        public int Id { get; set; }

        [MaxLength(2, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Prefijo { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]       
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int UltimoNumero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Longitud {  get; set; }

        [Display(Name = "Fecha de Creación")]       
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
