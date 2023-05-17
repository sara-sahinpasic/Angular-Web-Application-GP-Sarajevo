import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, take, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserRegisterRequest } from 'src/app/models/User/UserRegisterRequest';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  public userRequest: UserRegisterRequest = {
    firstName: "",
    lastName: "",
    password: "",
    dateOfBirth: new Date(),
    email: "",
    phoneNumber: ""
  };

  registrationSent:  boolean = false;

  constructor(private _router: Router, private userService: UserService) { }

  ngOnInit() {
    this.userService.isRegistrationSent.pipe(
      tap((val: boolean) => this.registrationSent = val)
    )
    .subscribe();
  }

  registracija() {
    this.userService.register(this.userRequest);
  }
}
