import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserLoginRequest } from 'src/app/models/User/UserLoginRequest';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.scss']
})
export class LogInComponent implements OnInit {

  isVerifyLoginRequestSent: boolean = false;
  enableVerifyButton: boolean = false;
  userLoginRequest: UserLoginRequest = {};
  userId?: string;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.userService.hasUserSentVerifyRequest$.pipe(
      tap((val: boolean) => this.isVerifyLoginRequestSent = val)
    )
    .subscribe();
  }

  login() {
    this.userService.login(this.userLoginRequest).subscribe();
  }

  verifyLogin(code: string) {
    this.userService.verifyLogin(Number(code), '').subscribe();
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
