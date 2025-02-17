using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaBiblioteca.Data;
using PracticaBiblioteca.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PracticaBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
        {
            return await _context.Libros.ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libros
                                      .Include(l => l.Autor) 
                                      .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
                return NotFound();

            return libro;
        }

       
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
                return BadRequest();

            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Libros.Any(l => l.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        //-------------
        [HttpGet("publicadosDespuesDe2000")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPublicadosDespuesDe2000()
        {
            var libros = await _context.Libros
                                        .Where(l => l.AñoPublicacion > 2000)
                                        .ToListAsync();

            if (!libros.Any())
                return NotFound("No se encontraron libros publicados después del año 2000.");

            return libros;
        }

        //----------------
        [HttpGet("{id}/cantidadLibros")]
        public async Task<ActionResult<int>> GetCantidadLibrosPorAutor(int id)
        {
            var cantidadLibros = await _context.Libros
                                                .Where(l => l.AutorId == id)
                                                .CountAsync();

            return cantidadLibros;
        }


        //-------------------
        [HttpGet("paginacion")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPaginados(int pageNumber = 1, int pageSize = 10)
        {
            var libros = await _context.Libros
                                        .Skip((pageNumber - 1) * pageSize) 
                                        .Take(pageSize)                     
                                        .ToListAsync();

            if (!libros.Any())
                return NotFound("No se encontraron libros en esta página.");

            return libros;
        }

    }
}
