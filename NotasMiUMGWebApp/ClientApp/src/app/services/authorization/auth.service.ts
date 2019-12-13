import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../environments/environment';

interface IUser {
  username: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private static API_LOGIN_URL = `${environment.apiBaseUrl}/api/login`;

  private jwtHelper: JwtHelperService = new JwtHelperService();
  private usuario: IUser = null;

  constructor(private router: Router, private http: HttpClient) { }

  // devuelve al usuario autenticado
  public getUser(): IUser {
    this.getUserFromStorage();
    const valido: boolean = this.tokenValido();
    if(!valido && this.usuario != null) {
      localStorage.removeItem("usuario");
      this.usuario = null;
    }
    return this.usuario;
  }

  // devuelve true si hay un usuario autenticado, false de lo contrario
  public isAuthenticated(): boolean {
    return this.getUser() != null;
  }

  // autentica a un usuario con usuario y contraseña
  public iniciarSesion(username: string, password: string): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      if (!this.isAuthenticated()) {
        this.http.post(AuthService.API_LOGIN_URL, {username, password})
          .subscribe((res: any) => {
            this.usuario = {username, token: res.token};
            this.saveUserToStorage();
            resolve(this.usuario);
          }, (err) => {
            reject({
              status: err.status,
              message: err.statusText,
              error: err.message
            });
          });

      } else {
        reject({status: 400, message: 'Ya has iniciado sesion', error: 'No puedes volver a iniciar sesion'});
      }
    });
  }

  // cierra la "sesión" del usuario
  public salir(redirect: boolean = true): void {
    if(this.getUser() != null) {
      console.log(`¡Hasta pronto ${this.usuario.username}!`);
      localStorage.removeItem("usuario");
      this.usuario = null;
    }
    if(redirect) {
      this.router.navigateByUrl('/login');
    }
  }

  // si usuario es null, trata de recuperar al usuario del localstorage
  private getUserFromStorage(): void {
    if(this.usuario != null) return;
    const usuarioStr: string = localStorage.getItem("usuario");
    if(usuarioStr == null) return;
    this.usuario = JSON.parse(usuarioStr);
  }

  // almacena el usuario en el localstorage
  private saveUserToStorage(): void {
    if(this.usuario == null) return;
    localStorage.setItem("usuario", JSON.stringify(this.usuario));
  }

  // devuelve true si el token es valido(no ha expirado)
  private tokenValido(): boolean {
    if(this.usuario == null) return false;
    return !this.jwtHelper.isTokenExpired(this.usuario.token);
  }

}
