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

  public loginForm = new FormGroup({
    username: new FormControl('', [
      Validators.required,
      Validators.email,
      Validators.minLength(14)
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
    const { username, password } = this.loginForm.value;
    this.auth.iniciarSesion(username, password)
      .then((res: any) => {
        console.log(`¡Bienvenido ${res.username}!`);
        this.router.navigateByUrl('/');
      })
      .catch(console.error);
  }

}
