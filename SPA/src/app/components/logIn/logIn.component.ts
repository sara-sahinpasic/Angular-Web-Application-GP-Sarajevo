import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserLoginRequest } from 'src/app/models/user/userLoginRequest';
import { CacheService } from 'src/app/services/cache/cache.service';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.scss']
})
export class LogInComponent implements OnInit {
  protected isVerifyLoginRequestSent: boolean = false;
  protected formGroup: FormGroup = {} as FormGroup;
  private userLoginRequest: UserLoginRequest = {} as UserLoginRequest;

  constructor(
    private router: Router,
    private userService: UserService,
    protected localizationService: LocalizationService,
    private cacheService: CacheService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.initializeForm()
  }

  private initializeForm() {
    this.formGroup = this.formBuilder.group({
      email: ['', Validators.compose([Validators.required, Validators.pattern(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/)])],
      password: ['', Validators.required]
    });
  }

  protected login() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    this.userLoginRequest.email = this.formGroup.get('email')?.value;
    this.userLoginRequest.password = this.formGroup.get('password')?.value;

    let redirectionRoute: string | null = this.cacheService.getDataFromCache('redirectUrl');

    this.userService.hasUserSentVerifyRequest$.pipe(
      tap((val: boolean) => this.isVerifyLoginRequestSent = val)
      )
      .subscribe();

    if (!redirectionRoute) {
      this.userService.login(this.userLoginRequest).subscribe();
      return;
    }

    this.userService.login(this.userLoginRequest, redirectionRoute).subscribe();
    this.cacheService.unsetCacheItem('redirectUrl');
  }

  protected navigateToRegistration() {
    this.router.navigateByUrl('/register');
  }
}
