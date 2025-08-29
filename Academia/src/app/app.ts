import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  weatherForecastService = inject(WeatherForecastService);
  climas: any [] = [];

  constructor(){
    this.weatherForecastService.obtenerClima().subscribe(datos => {
      this.climas =datos;
    });
  }
}
