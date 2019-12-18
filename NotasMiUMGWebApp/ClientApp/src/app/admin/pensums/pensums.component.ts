import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faTrash, faSave } from '@fortawesome/free-solid-svg-icons';
import { CarreraService } from '../../services/carrera.service';
import { CursoService } from '../../services/curso.service';
import { PensumService } from '../../services/pensum.service';
import ServerResponse from '../../model/ServerResponse';
import { Carrera } from '../../model/Carrera';


@Component({
  selector: 'app-pensums',
  templateUrl: './pensums.component.html',
  styleUrls: ['./pensums.component.css']
})
export class PensumsComponent implements OnInit {

  faTrash = faTrash;
  faSave = faSave;

  private carreras: Carrera[] = [];
  private codigoCarreraSel = 0;

  private pensums: number[] = [];
  private pensumSel: number = null;

  public formNewPensum = new FormGroup({
    anoPensum: new FormControl('', [
      Validators.required,
      Validators.pattern('^\\d+$')
    ])
  });

  constructor(
    private carreraService: CarreraService,
    private cursoService: CursoService,
    private pensumService: PensumService
  ) { }

  ngOnInit() {
    this.cargarCarreras();
  }

  public crearPensum(): void {

    if(this.codigoCarreraSel == 0) return;
    const newPensum = this.formNewPensum.value;
    newPensum.codigoCarrera = this.codigoCarreraSel * 1;
    newPensum.anoPensum *= 1;

    console.log(newPensum);

    this.pensumService.create(newPensum)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.pensums.unshift(newPensum.anoPensum);
          this.formNewPensum.reset();
        }
      }, console.error);
  }

  public eliminarPensum(): void {
    if(this.codigoCarreraSel == 0 || this.pensumSel == null) return;
    this.pensumService.delete(this.codigoCarreraSel, this.pensumSel)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.pensums.splice(this.pensums.findIndex(p => p == this.pensumSel), 1);
          this.pensumSel = null;
        }
      }, console.error);
  }

  public cargarPensums(): void {
    this.pensumSel = null;
    this.pensumService.getAllByCarrera(this.codigoCarreraSel)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.pensums = res.data.pensums;
        }
      }, console.error);
  }

  private cargarCarreras(): void {
    this.carreraService.getAll()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.carreras = res.data;
        }
      }, console.error);
  }

  public clickPensum(pensum: number): void {
    if(this.pensumSel == pensum) {
      this.pensumSel = null;
    } else {
      this.pensumSel = pensum;
    }
  }

}
