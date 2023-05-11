import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, of, Subscription, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserLoginRequest } from 'src/app/models/User/UserLoginRequest';
import { UserLoginResponse } from 'src/app/models/User/UserLoginResponse';
import { UserVerifyLoginRequest } from 'src/app/models/User/UserVerifyLoginRequest';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.scss']
})
export class LogInComponent implements OnInit {

  hasLoggedIn: boolean = false;
  enableVerifyButton: boolean = false;
  userLoginRequest: UserLoginRequest = {};
  userVerifyLoginRequest!: UserVerifyLoginRequest;
  userId?: string;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {}

  async login() {
    this.userId = await this.userService.login(this.userLoginRequest);

    if (this.userId != undefined) {
      this.hasLoggedIn = true;
    }
  }

  async verifyLogin(code: string) {
    if (this.userId == undefined) {
      return; // todo: error handling
    }

    this.userVerifyLoginRequest = {
      code: Number(code),
      userId: this.userId
    }
    const isSuccesfull: boolean = await this.userService.verifyLogin(this.userVerifyLoginRequest!);

    if (isSuccesfull) {
      this.router.navigateByUrl("");
      // todo: toast notification of succesfull login
    }
  }

  validateCodeLength(code: string) {
    if (code.length === 4) {
      this.enableVerifyButton = true;
      return;
    }

    this.enableVerifyButton = false;
  }

  navigateToRegistration() {
    this.router.navigateByUrl("/registracija");
  }
}
