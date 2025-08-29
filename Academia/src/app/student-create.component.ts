import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ApiService } from './api.service';

@Component({
  selector: 'app-student-create',
  template: `
    <h2>Crear Estudiante</h2>
    <form [formGroup]="form" (ngSubmit)="submit()" style="display:grid;grid-template-columns:1fr 1fr;gap:8px;max-width:600px">
      <input formControlName="name" placeholder="Nombre" />
      <input type="number" formControlName="age" placeholder="Edad" />
      <input formControlName="document" placeholder="Documento" />
      <select formControlName="licenseType">
        <option value="">Tipo de licencia</option>
        <option *ngFor="let t of licenseTypes" [value]="t">{{t}}</option>
      </select>
      <div style="grid-column:1 / -1">
        <button type="submit" [disabled]="form.invalid">Crear</button>
      </div>
    </form>
    <p *ngIf="message">{{message}}</p>
  `
})
export class StudentCreateComponent {
  @Output() created = new EventEmitter<void>();
  message = '';
  licenseTypes = ['A1','A2','B1','B2','C1','C2'];
  form = this.fb.group({
    name: ['', Validators.required],
    age: [18, [Validators.required, Validators.min(14)]],
    document: ['', Validators.required],
    licenseType: ['', Validators.required]
  });
  constructor(private fb: FormBuilder, private api: ApiService) {}
  submit() {
    if (this.form.invalid) return;
    this.api.createStudent(this.form.value).subscribe({
      next: r => { this.message = 'Creado correctamente'; this.created.emit(); this.form.reset({age:18}); setTimeout(()=>this.message='','2000'); },
      error: e => { this.message = 'Error: ' + (e?.error || e?.statusText || ''); }
    });
  }
}
