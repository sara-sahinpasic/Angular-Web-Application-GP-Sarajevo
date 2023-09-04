import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserProfileModel } from '../models/User/UserProfileModel';
import { UserService } from '../services/user/user.service';

@Injectable({
  providedIn: 'root',
})
export class NotLoggedInGuard implements CanActivate {

  isLoggedIn: any;

  constructor(private userService: UserService, private router: Router) {
    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => this.isLoggedIn = user))
    .subscribe();
  }

  canActivate() {
    if (this.isLoggedIn == undefined) {
      return true;
    }
    else {
      this.router.navigateByUrl("");
      return false;
    }
  }
}
