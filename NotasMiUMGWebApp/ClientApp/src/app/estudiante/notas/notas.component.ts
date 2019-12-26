import { Component, OnInit } from '@angular/core';
import { faPlus, faTrashAlt, faFilter } from '@fortawesome/free-solid-svg-icons';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Nota } from '../../model/Nota';
import { NotaService } from '../../services/nota.service';
import ServerResponse from '../../model/ServerResponse';

@Component({
  selector: 'app-notas',
  templateUrl: './notas.component.html',
  styleUrls: ['./notas.component.css']
})
export class NotasComponent implements OnInit {

  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faFilter = faFilter;

  public notas: Nota[] = [];
  public notaSel: Nota = null;

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

  constructor(
    private notaService: NotaService
  ) { }

  ngOnInit() {
  }

  public imp(val: any): void {
    console.log(val);
  }

  public verNotas(): void {
    this.cargarNotas();
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

}
