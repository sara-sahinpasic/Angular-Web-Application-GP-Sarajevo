import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, of, Subscription, tap } from 'rxjs';
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
  userLoginResponse?: UserLoginResponse;
  userVerifyLoginRequest!: UserVerifyLoginRequest;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {}

  // todo: check if login successfull then change to the hasLoggedIn value
  login() {
    this.userService.login(this.userLoginRequest).pipe(
      map(r => this.userLoginResponse = r),
      tap(r => {
        this.hasLoggedIn = true;
        this.userVerifyLoginRequest = {
          userId: r.userId,
          code: 0
        }
      }),
      catchError(e => {
        console.error(e); // todo: take error message
        return of([]);
      })
    )
    .subscribe();
  }

  verifyLogin(code: string) {
    this.userVerifyLoginRequest.code = Number(code);
    this.userService.verifyLogin(this.userVerifyLoginRequest!)
      .pipe(
        tap(t => {
          console.log(t);
          localStorage.setItem("userKey", t.token);
        })
      )
      .subscribe();
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
