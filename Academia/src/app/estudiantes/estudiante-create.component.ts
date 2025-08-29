import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-estudiante-create',
  templateUrl: './estudiante-create.component.html'
})
export class EstudianteCreateComponent {
  estudiante = { nombre: '', edad: 0, documento: '', tipoLicencia: 'A1' };

  constructor(private http: HttpClient) {}

  crearEstudiante() {
    if(!this.estudiante.nombre || !this.estudiante.documento) {
      alert("Nombre y documento son obligatorios");
      return;
    }
    this.http.post('http://localhost:5000/api/estudiantes', this.estudiante)
      .subscribe(() => alert("Estudiante creado con éxito"));
  }
}
