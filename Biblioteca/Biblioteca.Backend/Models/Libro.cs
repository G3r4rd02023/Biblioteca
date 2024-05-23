using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Backend.Models
{
    public class Libro
    {
        public int Id { get; set; }

        public Categoria Categoria { get; set; }

        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Fecha de Publicación")]
        public DateTime FechaPublicacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Cantidad { get; set; }

       
        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
