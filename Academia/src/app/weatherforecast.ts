import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class Weatherforecast {
  
  constructor(){}
  private http = inject(HttpClient)
  private URLbase = environment.apiURL + '/WeatherForecast';

  public obtenerClima() {
    return this.http.get<any>(this.URLbase);
  }
}
