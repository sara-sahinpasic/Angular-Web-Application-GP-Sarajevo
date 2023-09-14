import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../services/user/user.service';

@Injectable({
  providedIn: 'root',
})
export class NotLoggedInGuard implements CanActivate {

  isLoggedIn: any;

  constructor(private userService: UserService, private router: Router) {}

  canActivate() {
    if (!this.userService.isLoggedIn()) {
      return true;
    }
    else {
      this.router.navigateByUrl("");
      return false;
    }
  }
}
