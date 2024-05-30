using Biblioteca.Backend.Enums;
using Biblioteca.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;


        public SeedDb(DataContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;

        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await ValidarUsuariosAsync("ADMIN", "superadmin@gmail.com", "Activo", Roles.Administrador);

        }

        private async Task<Usuario> ValidarUsuariosAsync(string nombreCompleto, string correo, string estado, Roles rolUsuario)
        {
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuarioExistente != null)
            {
                return usuarioExistente;
            }

            Usuario usuario = new()
            {
                NombreCompleto = nombreCompleto,
                Correo = correo,
                Clave = "1234",
                Estado = estado,
                RolUsuario = rolUsuario,
                FechaCreacion = DateTime.Now
            };

            usuario.Clave = BCrypt.Net.BCrypt.HashPassword(usuario.Clave);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

    }
}
