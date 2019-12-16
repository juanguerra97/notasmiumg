import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {CarreraService} from '../services/carrera.service';
import {PensumService} from '../services/pensum.service';
import {Carrera} from '../model/Carrera';
import ServerResponse from '../model/ServerResponse';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';

@Component({
  selector: 'app-registro',
  templateUrl: './registro.component.html',
  styleUrls: ['./registro.component.css']
})
export class RegistroComponent implements OnInit {

  public carreras: Carrera[] = [];
  public pensums: number[] = [];

  public username = '';

  public procesandoRegistro = false;

  public formRegistro = new FormGroup({
    correo: new FormControl('', [
      Validators.required,
      Validators.minLength(14),
      Validators.pattern(`^([a-zA-Z0-9ñÑáéíóúÁÉÍÓÚ_+-]+)@miumg.edu.gt$`)
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ]),
    password_rep: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ]),
    nombre: new FormControl('', [
      Validators.required
    ]),
    apellido: new FormControl('', [
      Validators.required
    ]),
    carne: new FormControl('', [
      Validators.required,
      Validators.pattern('^\\d{4}-\\d{2}-\\d{1,8}$')
    ]),
    codigoCarrera: new FormControl('', [
      Validators.required,
    ]),
    anoPensum: new FormControl('', [
      Validators.required
    ]),
    anoInicio: new FormControl('', [
      Validators.required
    ])
  });

  constructor(
    private carreraService: CarreraService,
    private pensumService: PensumService,
    private router: Router,
    private http: HttpClient
  ) { }

  ngOnInit() {
    this.cargarCarreras();

  }

  public registrar(): void {
    let datos = this.formRegistro.value;
    datos.codigoCarrera = datos.codigoCarrera * 1;
    datos.anoPensum = datos.anoPensum * 1;
    console.log(datos);
    if(datos.password != datos.password_rep) return;

    this.procesandoRegistro = true;
    this.http.post<ServerResponse>(`${environment.apiBaseUrl}/api/estudiante/signup`, datos)
      .subscribe((res: ServerResponse) => {
        this.procesandoRegistro = false;
        if(res.status == 200) {
          this.router.navigateByUrl('/login');
        }
      }, (error: any) => {
        console.error(error);
        this.procesandoRegistro = false;
      });
  }

  public cargarCarreras(): void {
    this.carreraService.getAll()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.carreras = res.data;
        }
      }, console.error);
  }

  public cargarPensums(): void {
    this.pensumService.getAllByCarrera(this.formRegistro.value.codigoCarrera)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.pensums = res.data.pensums;
        }
      }, console.error);
  }

  public correoValido(): boolean {
    const campoCorreo = this.formRegistro.get('correo');
    const valido: boolean = campoCorreo.valid;
    if(valido) {
      const CORREO_REGEX: RegExp = /^([a-zA-Z0-9ñÑáéíóúÁÉÍÓÚ_+-]+)@miumg.edu.gt$/ig;
      const result = CORREO_REGEX.exec(campoCorreo.value);
      if(result) {
        this.username = result[1];
      }
    }
    return valido;
  }

}
