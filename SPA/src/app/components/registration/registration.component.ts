import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
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
  }

  registracija() {
    const observer: Observable<string> = this.userService.register(this.userRequest);
    observer.subscribe(() => {
      this.registrationSent = true;
    });
  }
}
