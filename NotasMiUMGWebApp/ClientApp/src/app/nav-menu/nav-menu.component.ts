import { Component } from '@angular/core';
import { faUser, faAddressCard, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../services/authorization/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  public faUser = faUser;
  public faAddressCard = faAddressCard;
  public faSignOutAlt = faSignOutAlt;

  isExpanded = false;

  constructor(private auth: AuthService) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public isAuthenticated(): boolean {
    return this.auth.isAuthenticated();
  }

  public getUsername(): string {
    const user = this.auth.getUser();
    if(user != null) return user.username;
    return '';
  }

}
