using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Backend.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public Libro Libro { get; set; }
        public int LibroId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime FechaDevolucion { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string EstadoPrestamo { get; set; }
    }
}