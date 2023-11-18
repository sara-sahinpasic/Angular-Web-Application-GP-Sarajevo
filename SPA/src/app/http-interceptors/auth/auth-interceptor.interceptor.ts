import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpStatusCode,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { UserService } from 'src/app/services/user/user.service';
import { Router } from '@angular/router';
import { ToastMessageService } from 'src/app/services/toast/toast-message.service';
import { LocalizationService } from 'src/app/services/localization/localization.service';

@Injectable()
export class AuthInterceptorInterceptor implements HttpInterceptor {

  constructor(
    private userService: UserService,
    private router: Router,
    private toastMessageService: ToastMessageService,
    private localizationService: LocalizationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    const authRequest: HttpRequest<unknown> = request.clone({
      headers: request.headers.set("Authorization", `Bearer ${this.userService.getUserJwtToken()}`)
    });

    return next.handle(authRequest)
      .pipe(
        tap({
          error: this.logoutUserOnTokenExpiry.bind(this)
        })
      );
  }

  private logoutUserOnTokenExpiry(event: HttpEvent<unknown>) {
    if (!(event instanceof HttpErrorResponse)) {
      return;
    }

    const response: HttpErrorResponse = event as HttpErrorResponse;
    const userJwtToken: string | null = this.userService.getUserJwtToken();

    if (response.status == HttpStatusCode.Unauthorized && userJwtToken) {
      this.handleUnauthorizedAccess();
      return;
    }

    if (response.status == HttpStatusCode.Forbidden && userJwtToken) {
      this.toastMessageService.pushErrorMessage(this.localizationService.localize('general_forbidden'));
    }
  }

  private handleUnauthorizedAccess() {
    this.userService.logout();
    this.router.navigateByUrl("/login");

    this.toastMessageService.pushErrorMessage(this.localizationService.localize('general_session_expired'));
  }
}
