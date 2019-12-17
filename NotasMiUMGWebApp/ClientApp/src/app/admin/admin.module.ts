import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutes } from './admin.routes';
import { CarrerasComponent } from './carreras/carreras.component';
import { CursosComponent } from './cursos/cursos.component';
import { PensumsComponent } from './pensums/pensums.component';

@NgModule({
  declarations: [CarrerasComponent, CursosComponent, PensumsComponent],
  imports: [
    CommonModule,
    AdminRoutes
  ]
})
export class AdminModule { }
