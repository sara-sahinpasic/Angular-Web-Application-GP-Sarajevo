import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Role } from 'src/app/models/User/Role';
import { UserService } from 'src/app/services/user/user.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private userService: UserService) {}

  canActivate(): boolean {
    const role: Role | null = this.userService.getUserRole();

    return !!role && role.name.toLowerCase() === "admin";
  }

}
