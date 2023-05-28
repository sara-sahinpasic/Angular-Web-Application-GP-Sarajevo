import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, take, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserRegisterRequest } from 'src/app/models/User/UserRegisterRequest';
import { UserService } from 'src/app/services/user.service';

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
    dateOfBirth: new Date(),
    email: '',
    phoneNumber: '',
  };

  registrationSent: boolean = false;

  registrationForm!: FormGroup;
  submitted = false;

  constructor(
    private userService: UserService,
    private _router: Router,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.validateForm();

    this.userService.isRegistrationSent$
      .pipe(tap((val: boolean) => (this.registrationSent = val)))
      .subscribe();
  }

  registracija() {
    /*
    ToDo: Toast messages when input fields are empty
    */
    this.userService.register(this.userRequest).subscribe();
  }
  login() {
    this._router.navigateByUrl('prijava');
  }
  onSubmit() {
    console.log(this.registrationForm.value);
  }
  validateForm(): void {
    this.registrationForm = this.formBuilder.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        phoneNumber: ['', Validators.required],
        email: ['', Validators.required],
        password: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }
}
