import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import ServerResponse from '../model/ServerResponse';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EstadisticaService {

  private static readonly ESTADISTICA_API_URL = `${environment.apiBaseUrl}/api/estadisticas`;

  constructor(private http: HttpClient) { }

  public getResumen(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/resumen`);
  }

  public getPromedios(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/promedios`);
  }

  public getNotasFinales(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/notafinal`);
  }

  public getExamenesFinales(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/examenfinal`);
  }

  public getPrimerosParciales(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/primerparcial`);
  }

  public getSegundosParciales(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/segundoparcial`);
  }

  public getActividades(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/actividades`);
  }

  public getZonas(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/zona`);
  }

  public getCreditos(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/creditos`);
  }

}
