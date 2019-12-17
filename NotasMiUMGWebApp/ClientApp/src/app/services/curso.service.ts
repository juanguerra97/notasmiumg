import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import ServerResponse from '../model/ServerResponse';
import { environment } from '../../environments/environment';
import { Curso } from '../model/Curso';

@Injectable({
  providedIn: 'root'
})
export class CursoService {

  private static readonly CURSO_API_URL = `${environment.apiBaseUrl}/api/cursos`;

  constructor(private http: HttpClient) { }

  public getAll(codigoCarrera: number): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${CursoService.CURSO_API_URL}/${codigoCarrera}`);
  }

  public create(curso: Curso): Observable<ServerResponse> {
    return this.http.post<ServerResponse>(CursoService.CURSO_API_URL, curso);
  }

  public delete(codigoCarrera: number, codigoCurso: number): Observable<ServerResponse> {
    return this.http.delete<ServerResponse>(`${CursoService.CURSO_API_URL}/${codigoCarrera}/${codigoCurso}`);
  }

}
