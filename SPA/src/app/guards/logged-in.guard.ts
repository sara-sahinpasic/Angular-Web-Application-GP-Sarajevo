import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {
  constructor(private userService: UserService) { }

  isLoggedIn: any = false;

  canActivate() {
    this.isLoggedIn = this.userService.getUser();

    if (this.isLoggedIn != undefined) {
      return true;
    }
    else {
      return false;
    }
  }
}
