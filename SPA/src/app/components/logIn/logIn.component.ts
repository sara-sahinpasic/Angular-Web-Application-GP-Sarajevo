import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, TitleStrategy } from '@angular/router';
import { tap } from 'rxjs';
import { UserLoginRequest } from 'src/app/models/User/UserLoginRequest';
import { CacheService } from 'src/app/services/cache/cache.service';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

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
  protected formGroup!: FormGroup;

  constructor(
    private router: Router,
    private userService: UserService,
    protected localizationService: LocalizationService,
    private cacheService: CacheService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.userService.hasUserSentVerifyRequest$.pipe(
      tap((val: boolean) => this.isVerifyLoginRequestSent = val)
    )
    .subscribe();
    this.initializeValidators()
  }

  initializeValidators() {
    this.formGroup = this.formBuilder.group({
      email: ["", Validators.compose([Validators.required, Validators.pattern(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/)])],
      password: ["", Validators.required]
    });
  }

  login() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    this.userLoginRequest.email = this.formGroup.get("email")?.value;
    this.userLoginRequest.password = this.formGroup.get("password")?.value;

    let redirectionRoute: string | null = this.cacheService.getDataFromCache("redirectUrl");

    if (!redirectionRoute) {
      this.userService.login(this.userLoginRequest).subscribe();
      return;
    }

    this.userService.login(this.userLoginRequest, redirectionRoute).subscribe();
    this.cacheService.unsetCacheItem("redirectUrl");
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
