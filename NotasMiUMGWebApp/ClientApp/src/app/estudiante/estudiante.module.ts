import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { EstudianteRoutes } from './estudiante.routes';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    EstudianteRoutes,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ]
})
export class EstudianteModule { }
