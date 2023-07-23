import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserRegisterRequest } from 'src/app/models/User/UserRegisterRequest';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})
export class RegistrationComponent implements OnInit {
  public userRequest: UserRegisterRequest = {
    firstName: '',
    lastName: '',
    password: '',
    dateOfBirth: '',
    email: '',
    phoneNumber: '',
  };

  registrationSent: boolean = false;
  registrationForm!: FormGroup;
  submitted: boolean = false;

  constructor(
    private userService: UserService,
    private _router: Router,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.initForm();

    this.userService.isRegistrationSent$
      .pipe(tap((val: boolean) => (this.registrationSent = val)))
      .subscribe();
  }

  registration() {
    /*
    ToDo: Toast messages when input fields are empty
    */
    this.registrationForm.markAllAsTouched();
    if (this.registrationForm.valid) {
      this.userService.register(this.userRequest).subscribe();
    }
  }

  login() {
    this._router.navigateByUrl('prijava');
  }

  initForm() {
    this.registrationForm = this.formBuilder.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        phoneNumber: ['', Validators.pattern('[- +()0-9]+')],
        email: ['', Validators.pattern(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/)],
        password: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }
}
