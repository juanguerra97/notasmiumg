import { Component, OnInit } from '@angular/core';
import { CarreraService } from '../../services/carrera.service';
import { Carrera } from '../../model/Carrera';
import ServerResponse from '../../model/ServerResponse';

@Component({
  selector: 'app-carreras',
  templateUrl: './carreras.component.html',
  styleUrls: ['./carreras.component.css']
})
export class CarrerasComponent implements OnInit {

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

  public seleccionarCarrera(carrera: Carrera): void {
    if(this.carreraSel == carrera){
      this.carreraSel = null;
    } else {
      this.carreraSel = carrera;
    }
  }

}
