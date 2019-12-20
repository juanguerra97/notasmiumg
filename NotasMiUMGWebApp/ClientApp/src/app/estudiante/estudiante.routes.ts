import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotasComponent } from './notas/notas.component';
import { EstadisticasComponent } from './estadisticas/estadisticas.component';

export const ESTUD_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'notas'
  },
  {
    path: 'notas',
    component: NotasComponent
  },
  {
    path: 'estadisticas',
    component: EstadisticasComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(ESTUD_ROUTES)],
  exports: [RouterModule]
})
export class EstudianteRoutes {}
