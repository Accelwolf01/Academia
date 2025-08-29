import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { EstudiantesListComponent } from './estudiantes/estudiantes-list.component';
import { EstudianteCreateComponent } from './estudiantes/estudiante-create.component';

@NgModule({
  declarations: [AppComponent, EstudiantesListComponent, EstudianteCreateComponent],
  imports: [BrowserModule, HttpClientModule, FormsModule],
  bootstrap: [AppComponent]
})
export class AppModule {}
