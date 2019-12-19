import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faTrash, faSave } from '@fortawesome/free-solid-svg-icons';
import { CarreraService } from '../../services/carrera.service';
import { CursoService } from '../../services/curso.service';
import { PensumCursoService } from '../../services/pensum-curso.service';
import { PensumService } from '../../services/pensum.service';
import ServerResponse from '../../model/ServerResponse';
import { Carrera } from '../../model/Carrera';
import { Curso } from '../../model/Curso';
import { PensumCurso } from '../../model/PensumCurso';

@Component({
  selector: 'app-pensums',
  templateUrl: './pensums.component.html',
  styleUrls: ['./pensums.component.css']
})
export class PensumsComponent implements OnInit {

  faTrash = faTrash;
  faSave = faSave;

  public carreras: Carrera[] = [];
  public codigoCarreraSel = 0;

  public pensums: number[] = [];
  public pensumSel: number = null;

  public cursos: Curso[] = [];

  public pensumCursos: PensumCurso[] = [];
  public pensumCursoSel: PensumCurso = null;

  public formNewPensum = new FormGroup({
    anoPensum: new FormControl('', [
      Validators.required,
      Validators.pattern('^\\d+$')
    ])
  });

  public formNewPensumCurso = new FormGroup({
    codigoCurso: new FormControl('',[
      Validators.required,
      Validators.pattern("^\\d+$")
    ]),
    ciclo: new FormControl('', [
      Validators.required,
      Validators.min(1),
      Validators.max(100),
      Validators.pattern("^\\d+$")
    ]),
    creditos: new FormControl('', [
      Validators.required,
      Validators.min(0),
      Validators.max(100),
      Validators.pattern("^\\d+$")
    ])
  });

  public verCursosChecked = false;

  constructor(
    private carreraService: CarreraService,
    private cursoService: CursoService,
    private pensumCursoService: PensumCursoService,
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

  public crearPensumCurso(): void {

    if(this.codigoCarreraSel == 0
      || this.pensumSel == null) return;

    const newPensumCurso = this.formNewPensumCurso.value;
    newPensumCurso.codigoCarrera = this.codigoCarreraSel * 1;
    newPensumCurso.anoPensum = this.pensumSel;
    newPensumCurso.codigoCurso *= 1;
    newPensumCurso.ciclo *= 1;
    newPensumCurso.creditos *= 1;

    this.pensumCursoService.create(newPensumCurso)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.cargarPensumCursosDePensum();
          this.formNewPensumCurso.reset();
        }
      }, console.error);
  }

  public eliminarPensumCurso(): void {
    if(this.codigoCarreraSel == 0
      || this.pensumSel == null
      || this.pensumCursoSel == null) return;
    this.pensumCursoService.delete(this.codigoCarreraSel, this.pensumSel, this.pensumCursoSel.codigoCurso)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.pensumCursos.splice(this.pensumCursos.findIndex(c => c.codigoCurso == this.pensumCursoSel.codigoCurso), 1);
          this.pensumCursoSel = null;
        }
      }, console.error);
  }

  public changeCarrera(): void {
    this.cargarPensums();
    if(this.verCursosChecked) {
      this.cargarCursos();
    } else {
      this.cursos = [];
    }
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
      this.pensumCursos = [];
      this.pensumCursoSel = null;
    } else {
      this.pensumSel = pensum;
      this.cargarPensumCursosDePensum();
    }
  }

  public changeVerPensumCursos(): void {
    this.pensumCursoSel = null;
    this.pensumCursos = [];
    if(this.verCursosChecked) {
      this.cargarCursos();
      if(this.pensumSel != null) {
        this.cargarPensumCursosDePensum();
      }
    }
  }

  public cargarPensumCursosDePensum(): void {
    this.pensumCursoService.getAllByPensum(this.codigoCarreraSel, this.pensumSel)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.pensumCursos = res.data;
        }
      }, console.error);
  }

  public clickPensumCurso(pensumCurso: PensumCurso): void {
    if(this.pensumCursoSel == pensumCurso) {
      this.pensumCursoSel = null;
    } else {
      this.pensumCursoSel = pensumCurso;
    }
  }

  public cargarCursos(): void {
    this.cursoService.getAll(this.codigoCarreraSel)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.cursos = res.data;
        }
      }, console.error);
  }

}
