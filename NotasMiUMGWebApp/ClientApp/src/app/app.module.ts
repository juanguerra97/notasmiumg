import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RegistroComponent } from './registro/registro.component';

import { NotLogedInGuard } from './not-loged-in.guard';
import { LogedInGuard } from './loged-in.guard';
import { AdminGuard } from './admin.guard';
import { EstudianteGuard } from './estudiante.guard';
import { TokenInterceptor } from './token.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    LogoutComponent,
    RegistroComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'login', component: LoginComponent, canActivate: [NotLogedInGuard]},
      {path: 'logout', component: LogoutComponent, canActivate: [LogedInGuard]},
      {path: 'registro', component: RegistroComponent, canActivate: [NotLogedInGuard]},
      {
        path: 'admin', canLoad: [AdminGuard], canActivateChild: [AdminGuard],
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
      },
      {
        path: 'estudiante', canLoad: [EstudianteGuard], canActivateChild: [EstudianteGuard],
        loadChildren: () => import('./estudiante/estudiante.module').then(m => m.EstudianteModule)
      },
    ]),
    ReactiveFormsModule,
    FontAwesomeModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
