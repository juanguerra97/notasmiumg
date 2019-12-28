import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import ServerResponse from '../model/ServerResponse';

@Injectable({
  providedIn: 'root'
})
export class EstudianteService {

  private static readonly ESTUDIANTE_API_URL = `${environment.apiBaseUrl}/api/estud`;

  constructor(private http: HttpClient) { }

  public get(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(EstudianteService.ESTUDIANTE_API_URL);
  }

}
