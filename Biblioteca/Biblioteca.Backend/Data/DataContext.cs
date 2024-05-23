using Biblioteca.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Correlativo> Correlativos { get; set; }

        public DbSet<Libro> Libros { get; set; }

        public DbSet<Prestamo> Prestamos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
