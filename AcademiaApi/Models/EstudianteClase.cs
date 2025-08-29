namespace AcademiaApi.Models {
    public class EstudianteClase {
        public int ID { get; set; }
        public int EstudianteID { get; set; }
        public Estudiante Estudiante { get; set; }
        public int ClaseID { get; set; }
        public Clase Clase { get; set; }
        public bool Activa { get; set; }
    }
}
