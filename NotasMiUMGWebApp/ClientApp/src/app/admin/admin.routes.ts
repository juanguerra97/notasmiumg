import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import { CarrerasComponent } from './carreras/carreras.component';
import { CursosComponent } from './cursos/cursos.component';

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
  }
];

@NgModule({
  imports: [RouterModule.forChild(ADMIN_ROUTES)],
  exports: [RouterModule]
})
export class AdminRoutes {}
