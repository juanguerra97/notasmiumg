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

  public getPromedios(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${EstadisticaService.ESTADISTICA_API_URL}/promedios`);
  }

}
