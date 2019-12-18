import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import ServerResponse from '../model/ServerResponse';
import { PensumCurso } from '../model/PensumCurso';


@Injectable({
  providedIn: 'root'
})
export class PensumCursoService {

  private static PENSUMCURSO_API_URL = `${environment.apiBaseUrl}/api/pensumcurso`;

  constructor(private http: HttpClient) { }

  public getAllByPensum(codigoCarrera: number, anoPensum: number): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${PensumCursoService.PENSUMCURSO_API_URL}/cursos/${codigoCarrera}/${anoPensum}`);
  }

  public getAllByCurso(codigoCarrera: number, codigoCurso: number): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${PensumCursoService.PENSUMCURSO_API_URL}/pensums/${codigoCarrera}/${codigoCurso}`);
  }

  public create(pensumCurso: PensumCurso): Observable<ServerResponse> {
    return this.http.post<ServerResponse>(PensumCursoService.PENSUMCURSO_API_URL, pensumCurso);
  }

  public delete(codigoCarrera: number, anoPensum: number, codigoCurso: number): Observable<ServerResponse> {
    return this.http.delete<ServerResponse>(`${PensumCursoService.PENSUMCURSO_API_URL}/${codigoCarrera}/${anoPensum}/${codigoCurso}`);
  }

  public update(pensumCurso: PensumCurso): Observable<ServerResponse> {
    return this.http.put<ServerResponse>(PensumCursoService.PENSUMCURSO_API_URL, pensumCurso);
  }

}
