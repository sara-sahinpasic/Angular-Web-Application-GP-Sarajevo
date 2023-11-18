import { Component } from '@angular/core';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-verify-login',
  templateUrl: './verify-login.component.html',
  styleUrls: ['./verify-login.component.scss']
})
export class VerifyLoginComponent {
  protected isVerifyButtonEnabled: boolean = false;

  constructor(private userService: UserService, protected localizationService: LocalizationService) { }

  protected verifyLogin(code: string) {
    this.userService.verifyLogin(Number(code), '')
      .subscribe();
  }

  protected validateCodeLength(code: string) {
    if (code.length === 4) {
      this.isVerifyButtonEnabled = true;
      return;
    }

    this.isVerifyButtonEnabled = false;
  }
}
