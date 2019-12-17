import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import { CarrerasComponent } from './carreras/carreras.component';
import { CursosComponent } from './cursos/cursos.component';
import { PensumsComponent } from './pensums/pensums.component';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'carreras'
  },
  {
    path: 'carreras',
    component: CarrerasComponent
  },
  {
    path: 'cursos',
    component: CursosComponent
  },
  {
    path: 'pensums',
    component: PensumsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(ADMIN_ROUTES)],
  exports: [RouterModule]
})
export class AdminRoutes {}
