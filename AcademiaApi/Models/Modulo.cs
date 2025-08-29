using System.Collections.Generic;

namespace AcademiaApi.Models {
    public class Modulo {
        public int ModuloID { get; set; }
        public string Nombre { get; set; }
        public ICollection<Clase> Clases { get; set; }
    }
}
