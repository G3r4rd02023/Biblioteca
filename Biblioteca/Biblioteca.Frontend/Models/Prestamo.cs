using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Frontend.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public Libro Libro { get; set; }
        public int LibroId { get; set; }
        public DateTime FechaPrestamo { get; set; } 
        public DateTime FechaDevolucion { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string EstadoPrestamo { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> Libros { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> Clientes { get; set; }


    }
}
