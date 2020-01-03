import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { EstudianteRoutes } from './estudiante.routes';
import { NotasComponent } from './notas/notas.component';
import { EstadisticasComponent } from './estadisticas/estadisticas.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ChartsModule } from 'ng2-charts';

@NgModule({
  declarations: [NotasComponent, EstadisticasComponent],
  imports: [
    CommonModule,
    EstudianteRoutes,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    NgbModule,
    ChartsModule
  ]
})
export class EstudianteModule { }
