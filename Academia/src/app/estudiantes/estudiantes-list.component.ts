import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-estudiantes-list',
  templateUrl: './estudiantes-list.component.html'
})
export class EstudiantesListComponent implements OnInit {
  estudiantes: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('http://localhost:5000/api/estudiantes')
      .subscribe(data => this.estudiantes = data);
  }
}
