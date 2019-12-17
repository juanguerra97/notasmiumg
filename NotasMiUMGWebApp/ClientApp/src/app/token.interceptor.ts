import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './services/authorization/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(
    private auth: AuthService
  ){}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const user = this.auth.getUser();
    if(user != null) {
      const modifiedReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${user.token}`),
      });
      return next.handle(modifiedReq);
    }
    return next.handle(req);
  }
}
