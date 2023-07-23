import { Injectable, OnInit } from '@angular/core';
import { CanActivate } from '@angular/router';
import { tap } from 'rxjs';
import { UserProfileModel } from '../models/User/UserProfileModel';
import { UserService } from '../services/user/user.service';

@Injectable({
  providedIn: 'root',
})
export class NotLoggedInGuard implements CanActivate {

  isLoggedIn: any;

  constructor(private userService: UserService) {
    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => this.isLoggedIn = user))
    .subscribe();
  }

  canActivate() {
    if (this.isLoggedIn == undefined) {
      return true;
    }
    else {
       return false;
    }
  }
}
