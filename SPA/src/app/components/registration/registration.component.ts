import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserRegisterRequest } from 'src/app/models/user/userRegisterRequest';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})
export class RegistrationComponent implements OnInit {
  protected userRequest: UserRegisterRequest = {} as UserRegisterRequest;
  protected isRegistrationSent: boolean = false;
  protected registrationForm: FormGroup = {} as FormGroup;

  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    protected localizationService: LocalizationService
  ) { }

  ngOnInit() {
    this.initializeForm();

    this.userService.isRegistrationSent$
      .pipe(tap((val: boolean) => (this.isRegistrationSent = val)))
      .subscribe();
  }

  protected register() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.userService.register(this.userRequest, '/login').subscribe();
    }
  }

  protected login() {
    this.router.navigateByUrl('login');
  }

  private initializeForm() {
    this.registrationForm = this.formBuilder.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        phoneNumber: ['', Validators.pattern('[- +()0-9]+')],
        email: ['', Validators.pattern(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/)],
        password: ['', Validators.minLength(8)],
      },
      { updateOn: 'change' }
    );
  }
}
