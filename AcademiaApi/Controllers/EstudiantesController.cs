using Microsoft.AspNetCore.Mvc;
using AcademiaApi.Data;
using AcademiaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase {
        private readonly AcademiaContext _context;

        public EstudiantesController(AcademiaContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes() {
            return await _context.Estudiantes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id) {
            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteClases)
                .ThenInclude(ec => ec.Clase)
                .ThenInclude(c => c.Modulo)
                .FirstOrDefaultAsync(e => e.EstudianteID == id);

            if (estudiante == null) return NotFound();
            return estudiante;
        }

        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante) {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.EstudianteID }, estudiante);
        }
    }
}
