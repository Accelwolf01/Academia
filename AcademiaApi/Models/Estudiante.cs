using System.Collections.Generic;

namespace AcademiaApi.Models {
    public class Estudiante {
        public int EstudianteID { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Documento { get; set; }
        public string TipoLicencia { get; set; }
        public ICollection<EstudianteClase> EstudianteClases { get; set; }
    }
}
