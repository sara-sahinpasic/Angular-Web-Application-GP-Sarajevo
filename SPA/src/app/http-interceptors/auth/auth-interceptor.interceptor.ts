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

@Injectable()
export class AuthInterceptorInterceptor implements HttpInterceptor {

  constructor(private userService: UserService, private router: Router, private toastMessageService: ToastMessageService) {}

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

    if (response.status == HttpStatusCode.Unauthorized && this.userService.getUserJwtToken()) {
      this.handleUnauthorizedAccess(); // todo: should be handled differently when we get the admin and driver roles up and running
    }
  }

  private handleUnauthorizedAccess() {
    this.userService.logout();
    this.router.navigateByUrl("/login");

    this.toastMessageService.pushErrorMessage('Session expired.');
  }
}
