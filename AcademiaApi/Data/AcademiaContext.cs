using Microsoft.EntityFrameworkCore;
using AcademiaApi.Models;

namespace AcademiaApi.Data {
    public class AcademiaContext : DbContext {
        public AcademiaContext(DbContextOptions<AcademiaContext> options) : base(options) { }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<EstudianteClase> EstudianteClases { get; set; }
    }
}
