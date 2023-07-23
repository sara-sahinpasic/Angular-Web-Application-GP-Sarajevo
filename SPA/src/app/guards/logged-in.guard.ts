import { Injectable, OnInit } from '@angular/core';
import { CanActivate } from '@angular/router';
import { UserProfileModel } from '../models/User/UserProfileModel';
import { tap } from 'rxjs';
import { UserService } from '../services/user/user.service';

@Injectable({
  providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {

  isLoggedIn: any;

  constructor(private userService: UserService) {
    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => this.isLoggedIn = user))
    .subscribe();
  }

  canActivate() {
    if (this.isLoggedIn != undefined) {
      return true;
    }
    else {
      return false;
    }
  }
}
