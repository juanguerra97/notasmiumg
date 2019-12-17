import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutes } from './admin.routes';
import { CarrerasComponent } from './carreras/carreras.component';

@NgModule({
  declarations: [CarrerasComponent],
  imports: [
    CommonModule,
    AdminRoutes
  ]
})
export class AdminModule { }
