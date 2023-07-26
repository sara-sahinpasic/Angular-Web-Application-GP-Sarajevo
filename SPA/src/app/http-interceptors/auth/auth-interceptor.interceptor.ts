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
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { Router } from '@angular/router';
import { ToastMessageService } from 'src/app/services/toast/toast-message.service';

@Injectable()
export class AuthInterceptorInterceptor implements HttpInterceptor {

  constructor(private userService: UserService, private router: Router, private toastMessageService: ToastMessageService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let userId: string = "";

    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => userId = user?.id ?? "")
    )
    .subscribe();

    request.headers.append("Authorization", `Bearer ${userId}`);

    return next.handle(request)
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

    if (response.status == HttpStatusCode.Unauthorized) {
      this.handleUnauthorizedAccess();
    }
  }

  private handleUnauthorizedAccess() {
    this.userService.logout();
    this.router.navigateByUrl("/prijava");

    this.toastMessageService.pushErrorMessage('Session expired.');
  }
}
