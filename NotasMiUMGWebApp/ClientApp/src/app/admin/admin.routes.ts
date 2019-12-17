import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';

export const ADMIN_ROUTES: Routes = [

];

@NgModule({
  imports: [RouterModule.forChild(ADMIN_ROUTES)],
  exports: [RouterModule]
})
export class AdminRoutes {}
