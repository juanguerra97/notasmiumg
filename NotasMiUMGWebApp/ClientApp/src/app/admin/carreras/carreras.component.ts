import { Component, OnInit } from '@angular/core';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
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

  public carreras: Carrera[] = [];
  public carreraSel: Carrera = null;

  constructor(private carreraService: CarreraService) { }

  ngOnInit() {
    this.carreraService.getAll()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.carreras = res.data;
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
