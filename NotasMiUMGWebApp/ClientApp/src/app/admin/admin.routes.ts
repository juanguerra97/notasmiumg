import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import { CarrerasComponent } from './carreras/carreras.component';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'carreras'
  },
  {
    path: 'carreras',
    component: CarrerasComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(ADMIN_ROUTES)],
  exports: [RouterModule]
})
export class AdminRoutes {}
