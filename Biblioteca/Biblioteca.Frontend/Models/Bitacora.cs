namespace Biblioteca.Frontend.Models
{
    public class Bitacora
    {
        public int Id { get; set; }

        public string Accion { get; set; }

        public string Tabla { get; set; }

        public Usuario Usuario { get; set; }

        public DateTime Fecha { get; set; }
    }
}
