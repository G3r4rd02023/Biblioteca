using Biblioteca.Backend.Data;
using Biblioteca.Backend.Models;
using Biblioteca.Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Biblioteca.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Registro")]
        public async Task<IActionResult> Registro([FromBody] Usuario model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Es recomendable hacer hash de la contraseña antes de guardarla
            model.Clave = BCrypt.Net.BCrypt.HashPassword(model.Clave);

            _context.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Usuario registrado exitosamente." });
        }

        [HttpPost("IniciarSesion")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var usuario = await _context.Usuarios
                .SingleOrDefaultAsync(u => u.Correo == login.NombreUsuario);          

            if (usuario != null)
            {                
                if (BCrypt.Net.BCrypt.Verify(login.Clave, usuario.Clave))
                {
                    // Aquí puedes agregar la lógica para generar un token JWT o manejar la sesión como prefieras
                    return Ok(new { Message = "Inicio de sesión exitoso." });
                }
            }

            return Unauthorized(new { Message = "Inicio de sesión fallido. Usuario o contraseña incorrectos." });
        }
    }
}
