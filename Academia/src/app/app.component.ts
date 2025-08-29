import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div style="max-width:800px;margin:2rem auto;font-family:Arial,Helvetica,sans-serif">
      <h1>Driving Academy - Estudiantes</h1>
      <app-student-create (created)="onCreated()"></app-student-create>
      <app-student-list></app-student-list>
    </div>
  `
})
export class AppComponent {
  onCreated() { /* placeholder to trigger reloads if needed */ }
}
