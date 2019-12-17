import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutes } from './admin.routes';
import { CarrerasComponent } from './carreras/carreras.component';
import { CursosComponent } from './cursos/cursos.component';

@NgModule({
  declarations: [CarrerasComponent, CursosComponent],
  imports: [
    CommonModule,
    AdminRoutes
  ]
})
export class AdminModule { }
