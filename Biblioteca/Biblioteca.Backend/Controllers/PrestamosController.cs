using Biblioteca.Backend.Data;
using Biblioteca.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly DataContext _context;

        public PrestamosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Prestamos
                .Include(x => x.Libro)
                .Include(x => x.Usuario)
                .ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(Prestamo prestamo)
        {
            if (prestamo == null)
            {
                return BadRequest("Prestamo no puede ser nulo.");
            }

            var libro = await _context.Libros.FindAsync(prestamo.LibroId);

            if (libro == null)
            {
                return NotFound("Libro no encontrado.");
            }

            if (prestamo.Cantidad <= 0)
            {
                return BadRequest("La cantidad de prestamo debe ser mayor que cero.");
            }

            if (libro.Cantidad < prestamo.Cantidad)
            {
                return BadRequest("No hay suficientes libros disponibles.");
            }

           

            libro.Cantidad -= prestamo.Cantidad;
            
            if (libro.Cantidad == 0)
            {
                libro.Estado = "NoDisponible";
            }

            prestamo.Id = 0;

            _context.Prestamos.Add(prestamo);
            _context.Libros.Update(libro);  
            await _context.SaveChangesAsync();
            
            return Ok();
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(x => x.Libro)
                .Include(x => x.Usuario)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }
            return Ok(prestamo);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return BadRequest();
            }

            var libro = await _context.Libros.FindAsync(prestamo.LibroId);

            if (libro == null)
            {
                return NotFound("Libro no encontrado.");
            }

            libro.Cantidad += prestamo.Cantidad;
            libro.Estado = "Disponible";
            _context.Update(prestamo);
            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            _context.Remove(prestamo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
