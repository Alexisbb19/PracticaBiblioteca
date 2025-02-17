using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaBiblioteca.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracticaBiblioteca.Data;


namespace PracticaBiblioteca.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly BibliotecaContext _context;

        public AutoresController(BibliotecaContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            return await _context.Autores.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var autor = await _context.Autores
                                      .Include(a => a.Libros) 
                                      .FirstOrDefaultAsync(a => a.Id == id);

            if (autor == null)
                return NotFound();

            return autor;
        }

        
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAutor), new { id = autor.Id }, autor);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, Autor autor)
        {
            if (id != autor.Id)
                return BadRequest();

            _context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Autores.Any(a => a.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
                return NotFound();

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        //
        [HttpGet("autoresConMasLibros")]
        public async Task<ActionResult<IEnumerable<object>>> GetAutoresConMasLibros()
        {
            var autores = await _context.Libros
                                         .GroupBy(l => l.AutorId)
                                         .Select(g => new
                                         {
                                             AutorId = g.Key,
                                             NombreAutor = _context.Autores.Where(a => a.Id == g.Key).Select(a => a.Nombre).FirstOrDefault(),
                                             CantidadLibros = g.Count()
                                         })
                                         .OrderByDescending(a => a.CantidadLibros)
                                         .ToListAsync();

            return autores;
        }


        //


    }
}
