using Microsoft.EntityFrameworkCore;
using DrivingAcademy.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=driving_academy.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS policy for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Ensure DB created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowAngular");

app.UseHttpsRedirection();


// CRUD for Students
app.MapGet("/api/students", async (AppDbContext db) =>
    await db.Students.Include(s => s.Enrollments).ThenInclude(e => e.Class).ToListAsync());

app.MapGet("/api/students/{id:int}", async (int id, AppDbContext db) =>
    await db.Students.Include(s => s.Enrollments).ThenInclude(e => e.Class)
        .FirstOrDefaultAsync(s => s.Id == id)
        is Student student ? Results.Ok(student) : Results.NotFound());

app.MapPost("/api/students", async (StudentDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name) || dto.LicenseType is null) 
        return Results.BadRequest("Name and LicenseType are required.");
    var student = new Student { Name = dto.Name, Age = dto.Age, Document = dto.Document, LicenseType = dto.LicenseType };
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/api/students/{student.Id}", student);
});

app.MapGet("/api/modules", async (AppDbContext db) =>
    await db.Modules.Include(m => m.Classes).ToListAsync());

app.MapGet("/api/classes", async (AppDbContext db) =>
    await db.Classes.ToListAsync());

// Enroll a student into a class (simple rules: not two active levels of same module)
app.MapPost("/api/enrollments", async (EnrollmentDto dto, AppDbContext db) =>
{
    var student = await db.Students.Include(s=>s.Enrollments).ThenInclude(e=>e.Class).FirstOrDefaultAsync(s=>s.Id==dto.StudentId);
    var cls = await db.Classes.Include(c=>c.Module).FirstOrDefaultAsync(c=>c.Id==dto.ClassId);
    if (student==null || cls==null) return Results.BadRequest("Invalid student or class.");
    // Check rule: student cannot be active in more than one level of same module
    var sameModuleActive = student.Enrollments.Any(e => e.Class.ModuleId == cls.ModuleId && e.IsActive);
    if (sameModuleActive) return Results.Conflict("Student already active in a level of this module.");
    var enrollment = new Enrollment { StudentId = student.Id, ClassId = cls.Id, IsActive = true, EnrolledAt = DateTime.UtcNow };
    db.Enrollments.Add(enrollment);
    await db.SaveChangesAsync();
    return Results.Ok(enrollment);
});

app.Run();

// ----------------- Models and DbContext -----------------

namespace DrivingAcademy.Api
{
    public enum LicenseType { A1, A2, B1, B2, C1, C2 }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string Document { get; set; } = default!;
        public LicenseType LicenseType { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }

    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<Class> Classes { get; set; } = new();
    }

    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int ModuleId { get; set; }
        public Module? Module { get; set; }
    }

    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int ClassId { get; set; }
        public Class? Class { get; set; }
        public bool IsActive { get; set; }
        public DateTime EnrolledAt { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasIndex(s=>s.Document).IsUnique(false);
            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Modules.Any()) return;

            var adapt = new Module { Name = "Adaptación" };
            var ethic = new Module { Name = "Ética" };
            var legal = new Module { Name = "Marco Legal" };

            adapt.Classes.Add(new Class { Name = "Adaptación 1" });
            adapt.Classes.Add(new Class { Name = "Adaptación 2" });

            ethic.Classes.Add(new Class { Name = "Ética 1" });
            ethic.Classes.Add(new Class { Name = "Ética 2" });

            legal.Classes.Add(new Class { Name = "Marco Legal 1" });

            db.Modules.AddRange(adapt, ethic, legal);

            db.Students.AddRange(
                new Student { Name = "Juan Pérez", Age=25, Document="1010", LicenseType=LicenseType.B1 },
                new Student { Name = "María Gómez", Age=30, Document="2020", LicenseType=LicenseType.A2 }
            );

            db.SaveChanges();

            // enroll Juan in Adaptación 1
            var j = db.Students.First();
            var a1 = db.Classes.First(c=>c.Name=="Adaptación 1");
            db.Enrollments.Add(new Enrollment { StudentId = j.Id, ClassId = a1.Id, IsActive = true, EnrolledAt = DateTime.UtcNow });

            db.SaveChanges();
        }
    }

    public record StudentDto(string Name, int Age, string Document, LicenseType LicenseType);
    public record EnrollmentDto(int StudentId, int ClassId);
}
