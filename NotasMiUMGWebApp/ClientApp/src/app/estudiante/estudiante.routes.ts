import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

export const ESTUD_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'notas'
  },
];

@NgModule({
  imports: [RouterModule.forChild(ESTUD_ROUTES)],
  exports: [RouterModule]
})
export class EstudianteRoutes {}
