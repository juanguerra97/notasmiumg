import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import ServerResponse from '../model/ServerResponse';

@Injectable({
  providedIn: 'root'
})
export class CarreraService {

  private static CARRERA_API_URL = `${environment.apiBaseUrl}/api/carreras`;

  constructor(private http: HttpClient) { }

  public getAll(): Observable<ServerResponse> {
    return this.http.get<ServerResponse>(CarreraService.CARRERA_API_URL);
  }

}
