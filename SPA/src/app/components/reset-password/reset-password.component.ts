import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  @ViewChild("email") resetPasswordInput?: ElementRef<HTMLInputElement>;
  protected isEmailValid: boolean | undefined = undefined;
  protected isResetPasswordRequestSent: boolean = false;
  protected formGroup!: FormGroup;

  constructor(private userService: UserService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.initializeValidators();
    this.userService.isResetPasswordRequestSent$.pipe(
      tap((val: boolean) => this.isResetPasswordRequestSent = val)
    )
      .subscribe();
  }

  sendResetPasswordRequest() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    const email: string = this.formGroup.get("email")!.value;
    this.userService.resetPassword(email).subscribe();
  }

  private initializeValidators() {
    this.formGroup = this.formBuilder.group({
      email: ["", Validators.compose([Validators.pattern(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/), Validators.required])]
    })
  }
}
