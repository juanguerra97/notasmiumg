import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faTrash, faSave } from '@fortawesome/free-solid-svg-icons';
import { CarreraService } from '../../services/carrera.service';
import { CursoService } from '../../services/curso.service';
import { Carrera } from '../../model/Carrera';
import { Curso } from '../../model/Curso';
import ServerResponse from '../../model/ServerResponse';



@Component({
  selector: 'app-cursos',
  templateUrl: './cursos.component.html',
  styleUrls: ['./cursos.component.css']
})
export class CursosComponent implements OnInit {

  faTrash = faTrash;
  faSave = faSave;

  private carreras: Carrera[] = [];
  private codigoCarreraSel = 0;

  private cursos: Curso[] = [];
  private cursoSel: Curso = null;

  public formNewCurso = new FormGroup({
    codigoCurso: new FormControl('', [
      Validators.required,
      Validators.pattern('^\\d+$')
    ]),
    nombreCurso: new FormControl('',[
      Validators.required
    ])
  });

  constructor(
    private carreraService: CarreraService,
    private cursoService: CursoService
  ) { }

  ngOnInit() {
    this.carreraService.getAll()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.carreras = res.data;
        }
      }, console.error);
  }

  public crearCurso(): void {

    if(this.codigoCarreraSel == 0) return;
    const newCurso = this.formNewCurso.value;
    newCurso.codigoCarrera = this.codigoCarreraSel * 1;
    newCurso.codigoCurso *= 1;
    newCurso.nombreCurso = newCurso.nombreCurso.trim().toUpperCase();

    if(newCurso.nombreCurso == '') return;

    console.log(newCurso);

    this.cursoService.create(newCurso)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.cursos.unshift(newCurso);
          this.formNewCurso.reset();
        }
      }, console.error);
  }

  public eliminarCurso(): void {
    if(this.codigoCarreraSel == 0 || this.cursoSel == null) return;
    this.cursoService.delete(this.codigoCarreraSel, this.cursoSel.codigoCurso)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.cursos.splice(this.cursos.findIndex(c => c.codigoCurso == this.cursoSel.codigoCurso), 1);
          this.cursoSel = null;
        }
      }, console.error);
  }

  public cargarCursos(): void {
    this.cursoSel = null;
    this.cursoService.getAll(this.codigoCarreraSel)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.cursos = res.data;
        }
      }, console.error);
  }

  public clickCurso(curso: Curso): void {
    if(this.cursoSel == curso) {
      this.cursoSel = null;
    } else {
      this.cursoSel = curso;
    }
  }

}
