import { Component, OnInit } from '@angular/core';
import { faPlus, faTrashAlt, faFilter } from '@fortawesome/free-solid-svg-icons';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Nota } from '../../model/Nota';
import { NotaService } from '../../services/nota.service';
import ServerResponse from '../../model/ServerResponse';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CursoService } from '../../services/curso.service';
import { AuthService } from '../../services/authorization/auth.service';
import { Curso } from '../../model/Curso';
import { Estudiante } from '../../model/Estudiante';
import { EstudianteService } from '../../services/estudiante.service';

@Component({
  selector: 'app-notas',
  templateUrl: './notas.component.html',
  styleUrls: ['./notas.component.css']
})
export class NotasComponent implements OnInit {

  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faFilter = faFilter;

  public estudiante: Estudiante = null;

  public notas: Nota[] = [];
  public notaSel: Nota = null;

  public cursos: Curso[] = [];

  public filtroModificado = false;

  public formVerNotas = new FormGroup({
    ano: new FormControl('',[
      Validators.required,
      Validators.pattern('^\\d+$')
    ]),
    ciclo: new FormControl('',[
      Validators.required,
      Validators.pattern('^\\d+$')
    ])
  });

  public formNuevaNota = new FormGroup({
    codigoCurso: new FormControl('', [
      Validators.required
    ]),
    primerParcial: new FormControl('',[
      Validators.required,
      Validators.pattern('^\\d+$')
    ]),
    segundoParcial: new FormControl('',[
      Validators.required,
      Validators.pattern('^\\d+$')
    ]),
    actividades: new FormControl('',[
      Validators.required,
      Validators.pattern('^\\d+$')
    ]),
    examenFinal: new FormControl('',[
      Validators.required,
      Validators.pattern('^\\d+$')
    ])
  });

  constructor(
    private notaService: NotaService,
    private cursoService: CursoService,
    private estudianteService: EstudianteService,
    private authService: AuthService,
    private modalService: NgbModal,
  ) { }

  ngOnInit() {
    this.estudianteService.get()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200){
          this.estudiante = res.data;
        }
      }, console.error);
  }

  public onGuardarNota(): void {

    if(!this.formNuevaNota.valid || this.estudiante == null || !this.formVerNotas.valid) return;

    const newNota = this.formNuevaNota.value;
    // se convierten de string a numero
    newNota.codigoCurso *= 1;
    newNota.primerParcial *= 1;
    newNota.segundoParcial *= 1;
    newNota.actividades *= 1;
    newNota.examenFinal *= 1;

    // datos faltantes
    newNota.estudianteId = this.estudiante.estudianteId;
    newNota.codigoCarrera = this.estudiante.codigoCarrera;
    newNota.anoPensum = this.estudiante.anoPensum;
    newNota.ano = this.formVerNotas.value.ano;

    this.notaService.create(newNota)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.cargarNotas();
        }
      }, console.error);

    this.modalService.dismissAll();// cierra el modal
  }

  public openModalAgregarNota(content: any): void {
    if(!this.formVerNotas.valid) return;
    this.formNuevaNota.reset();
    this.modalService.open(content);
  }

  public imp(val: any): void {
    console.log(val);
  }

  public verNotas(): void {
    this.cargarNotas();
    this.cargarCursos();
    this.filtroModificado = false;
  }

  public cargarNotas(): void {
    if(!this.formVerNotas.valid) return;
    const {ano: ano, ciclo: ciclo} = this.formVerNotas.value;
    this.notaService.getAllByCiclo(ano, ciclo)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.notas = res.data;
        }
      }, console.error);
  }

  public cambioFiltro(): void {
    this.filtroModificado = true;
  }

  public cargarCursos(): void {
    if(this.estudiante == null) return;
    this.cursoService.getAllByCiclo(this.estudiante.codigoCarrera, this.estudiante.anoPensum, this.formVerNotas.value.ciclo)
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.cursos = res.data;
        }
      }, console.error);
  }

}
