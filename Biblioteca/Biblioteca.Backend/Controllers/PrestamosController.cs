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
            _context.Add(prestamo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(x => x.Libro)
                .Include(x => x.Usuario)
                .SingleOrDefaultAsync(c => c.Id == id);
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

            _context.Update(prestamo);
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
