import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/authorization/auth.service';

@Component({
  selector: 'app-logout',
  template: '',
  styleUrls: []
})
export class LogoutComponent implements OnInit {

  constructor(private auth: AuthService) { }

  ngOnInit() {
    this.auth.salir();
  }

  ng

}
