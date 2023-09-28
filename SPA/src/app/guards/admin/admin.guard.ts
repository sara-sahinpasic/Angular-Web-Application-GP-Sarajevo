import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Roles } from 'src/app/models/roles/roles';
import { UserService } from 'src/app/services/user/user.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private userService: UserService) {}

  canActivate(): boolean {
    return this.userService.isUserInRole(Roles.admin);
  }

}
