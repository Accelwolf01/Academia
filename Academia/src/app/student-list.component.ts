import { Component, OnInit } from '@angular/core';
import { ApiService, Student } from './api.service';

@Component({
  selector: 'app-student-list',
  template: `
    <h2>Listado</h2>
    <table border="1" cellpadding="6" style="border-collapse:collapse">
      <tr><th>Nombre</th><th>Edad</th><th>Documento</th><th>Licencia</th><th>Inscrito en</th></tr>
      <tr *ngFor="let s of students">
        <td>{{s.name}}</td><td>{{s.age}}</td><td>{{s.document}}</td><td>{{s.licenseType}}</td>
        <td>
          <ul>
            <li *ngFor="let e of s.enrollments">{{e?.class?.name || (e.classId)}}</li>
          </ul>
        </td>
      </tr>
    </table>
    <p *ngIf="students.length===0">No hay estudiantes aún.</p>
  `
})
export class StudentListComponent implements OnInit {
  students: Student[] = [];
  constructor(private api: ApiService) {}
  ngOnInit() { this.load(); }
  load() { this.api.getStudents().subscribe(r=>this.students = r); }
}
