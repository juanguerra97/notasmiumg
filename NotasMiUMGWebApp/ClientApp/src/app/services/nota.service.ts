import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import { environment } from '../../environments/environment';
import ServerResponse from '../model/ServerResponse';
import { Nota } from '../model/Nota';

@Injectable({
  providedIn: 'root'
})
export class NotaService {

  private static readonly NOTA_API_URL = `${environment.apiBaseUrl}/api/notas`;

  constructor(private http: HttpClient) { }

  public getAll(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(NotaService.NOTA_API_URL);
  }

  public getAllByAno(ano: number): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${NotaService.NOTA_API_URL}/${ano}`);
  }

  public getAllByCiclo(ano: number, ciclo: number): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${NotaService.NOTA_API_URL}/${ano}/${ciclo}`);
  }

  public create(nota: Nota): Observable<ServerResponse> {
    return this.http.post<ServerResponse>(NotaService.NOTA_API_URL, nota);
  }

  public delete(codigoCurso: number, ano: number): Observable<ServerResponse> {
    return this.http.delete<ServerResponse>(`${NotaService.NOTA_API_URL}/${codigoCurso}/${ano}`);
  }

  public update(nota: Nota): Observable<ServerResponse> {
    return this.http.put<ServerResponse>(NotaService.NOTA_API_URL, nota);
  }

}
