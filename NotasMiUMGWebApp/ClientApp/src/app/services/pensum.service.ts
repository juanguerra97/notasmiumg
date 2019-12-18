import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import ServerResponse from '../model/ServerResponse';
import Pensum from '../model/Pensum';

@Injectable({
  providedIn: 'root'
})
export class PensumService {

  private static PENSUM_API_URL = `${environment.apiBaseUrl}/api/pensums`;

  constructor(private http: HttpClient) { }

  public getAllByCarrera(codigoCarrera: number): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(`${PensumService.PENSUM_API_URL}/${codigoCarrera}`);
  }

  public create(pensum: Pensum): Observable<ServerResponse> {
    return this.http.post<ServerResponse>(PensumService.PENSUM_API_URL, pensum);
  }

  public delete(codigoCarrera: number, anoPensum: number): Observable<ServerResponse> {
    return this.http.delete<ServerResponse>(`${PensumService.PENSUM_API_URL}/${codigoCarrera}/${anoPensum}`);
  }

}
