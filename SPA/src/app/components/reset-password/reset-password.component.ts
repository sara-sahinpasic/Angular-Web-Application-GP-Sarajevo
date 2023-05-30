import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Observable, Subscription, debounceTime, fromEvent, tap } from 'rxjs';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild("email") resetPasswordInput?: ElementRef<HTMLInputElement>;
  private onKeyup$?: Observable<Event>;
  private subscription?: Subscription;
  protected isEmailValid: boolean | undefined = undefined;
  protected isResetPasswordRequestSent: boolean = false;

  constructor(private userSerice: UserService) { }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.onKeyup$ = fromEvent(this.resetPasswordInput?.nativeElement as HTMLInputElement, "keyup")
      .pipe(
        tap((val: Event) => {
          const target: HTMLInputElement = val.target as HTMLInputElement;

          this.isEmailValid = this.validateEmail(target.value as string);
        }),
        debounceTime(1000)
      );

    this.subscription = this.onKeyup$.subscribe();
  }

  ngOnInit() {
    this.userSerice.isResetPasswordRequestSent$.pipe(
      tap((val: boolean) => this.isResetPasswordRequestSent = val)
    )
      .subscribe();
  }

  sendResetPasswordRequest(email: string) {
    this.userSerice.resetPassword(email).subscribe();
  }

  private validateEmail(email: string): boolean {
    return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email);
  }
}
