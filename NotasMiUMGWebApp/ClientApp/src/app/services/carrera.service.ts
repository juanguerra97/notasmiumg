import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import ServerResponse from '../model/ServerResponse';
import {Carrera} from '../model/Carrera';

@Injectable({
  providedIn: 'root'
})
export class CarreraService {

  private static readonly CARRERA_API_URL = `${environment.apiBaseUrl}/api/carreras`;

  constructor(private http: HttpClient) { }

  public getAll(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(CarreraService.CARRERA_API_URL);
  }

  public create(carrera: Carrera): Observable<ServerResponse> {
    return this.http.post<ServerResponse>(CarreraService.CARRERA_API_URL, carrera);
  }

  public delete(codigoCarrera: number): Observable<ServerResponse> {
    return this.http.delete<ServerResponse>(`${CarreraService.CARRERA_API_URL}/${codigoCarrera}`);
  }

}
