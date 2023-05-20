import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  isEmailValid: boolean | undefined = undefined;

  constructor(private userSerice: UserService) { }

  ngOnInit() {
  }

  sendResetPasswordRequest(email: string) {
    this.userSerice.resetPassword(email);
  }

  validateEmail(email: string) {
    this.isEmailValid = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email);
  }
}
