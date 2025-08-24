import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../core/services/current-user.service';
import { User, TEAM_MEMBERS } from '../../../core/models/user.model';


@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  selectedUserId = '';
  wantAdmin = false;
  currentUser?: User;

  constructor(private currentUserService: CurrentUserService, private router: Router) {}

  ngOnInit() {
    this.currentUserService.loadFromStorage();
    this.currentUser = this.currentUserService.currentUser;
  }

  filteredMembers(): User[] {
    return this.wantAdmin
      ? TEAM_MEMBERS.filter(u => u.role === 'Admin')
      : TEAM_MEMBERS.filter(u => u.role !== 'Admin');
  }

  login() {
    const user = TEAM_MEMBERS.find(u => u.id === this.selectedUserId);
    if (!user) {
      alert('Please select a valid user.');
      return;
    }

    if (this.wantAdmin && user.role !== 'Admin') {
      alert('You must select the Admin name to log in as Admin.');
      return;
    }

    this.currentUserService.setCurrentUser(user);
    this.currentUser = user;
    this.router.navigate(['/tasks']);
  }

  logout() {
    this.currentUserService.logout();
    this.currentUser = undefined;
    this.selectedUserId = '';
    this.wantAdmin = false;
    this.router.navigate(['/login']);
  }
}
