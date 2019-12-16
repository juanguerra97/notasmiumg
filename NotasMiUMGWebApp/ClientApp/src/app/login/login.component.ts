import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {AuthService} from '../services/authorization/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public procesandoLogin = false;

  public loginForm = new FormGroup({
    username: new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.pattern(`^([a-zA-Z0-9ñÑáéíóúÁÉÍÓÚ_+-]+)(@miumg.edu.gt)?$`)
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ]),
  });

  constructor(
    private auth: AuthService,
    private router: Router
  ) { }

  ngOnInit() {

  }

  public iniciarSesion(): void {
    this.procesandoLogin = true;
    const { username, password } = this.loginForm.value;
    this.auth.iniciarSesion(username, password)
      .then((res: any) => {
        this.procesandoLogin = false;
        console.log(`¡Bienvenido ${res.username}!`);
        this.router.navigateByUrl('/');
      })
      .catch((error: any)=>{
        console.error(error);
        this.procesandoLogin = false;
      });
  }

}
