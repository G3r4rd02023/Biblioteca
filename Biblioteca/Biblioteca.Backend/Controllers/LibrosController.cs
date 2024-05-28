using Biblioteca.Backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Backend.Models;

namespace Biblioteca.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly DataContext _context;

        public LibrosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Libros
                .Include(l => l.Categoria)
                .ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Libro libro)
        {
            _context.Add(libro);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var libro = await _context.Libros
                 .Include(l => l.Categoria)
                .SingleOrDefaultAsync(c => c.Id == id);
            if (libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }

            _context.Update(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            _context.Remove(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
