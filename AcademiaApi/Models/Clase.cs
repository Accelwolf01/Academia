namespace AcademiaApi.Models {
    public class Clase {
        public int ClaseID { get; set; }
        public string Nombre { get; set; }
        public int ModuloID { get; set; }
        public Modulo Modulo { get; set; }
    }
}
