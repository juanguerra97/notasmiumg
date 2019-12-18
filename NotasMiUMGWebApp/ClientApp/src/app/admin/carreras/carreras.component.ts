import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { faTrash, faSave } from '@fortawesome/free-solid-svg-icons';
import { CarreraService } from '../../services/carrera.service';
import { Carrera } from '../../model/Carrera';
import ServerResponse from '../../model/ServerResponse';

@Component({
  selector: 'app-carreras',
  templateUrl: './carreras.component.html',
  styleUrls: ['./carreras.component.css']
})
export class CarrerasComponent implements OnInit {

  faTrash = faTrash;
  faSave = faSave;

  public carreras: Carrera[] = [];
  public carreraSel: Carrera = null;

  public formNewCarrera = new FormGroup({
    codigoCarrera: new FormControl('', [
      Validators.required,
      Validators.pattern('^\\d+$')
    ]),
    nombreCarrera: new FormControl('',[
      Validators.required
    ])
  });

  constructor(private carreraService: CarreraService) { }

  ngOnInit() {
    this.carreraService.getAll()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.carreras = res.data;
        }
      }, console.error);
  }

  public crearCarrera(): void {

    const newCarrera = this.formNewCarrera.value;
    newCarrera.nombreCarrera = newCarrera.nombreCarrera.trim().toUpperCase();

    if(newCarrera.nombreCarrera == '') return;

    this.carreraService.create(newCarrera)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.carreras.unshift(newCarrera);
          this.formNewCarrera.reset();
        }
      }, console.error);
  }

  public eliminarCarrera(): void {
    if(this.carreraSel == null) return;
    this.carreraService.delete(this.carreraSel.codigoCarrera)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.carreras.splice(this.carreras.findIndex(c => c.codigoCarrera === this.carreraSel.codigoCarrera), 1);
          this.carreraSel = null;
        }
      }, console.error);
  }

  public clickCarrera(carrera: Carrera): void {
    if(this.carreraSel == carrera){
      this.carreraSel = null;
    } else {
      this.carreraSel = carrera;
    }
  }

}
