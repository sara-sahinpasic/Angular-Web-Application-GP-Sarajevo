import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse,
  HttpStatusCode
} from '@angular/common/http';
import { Observable, finalize, tap } from 'rxjs';
import { ToastMessageService } from '../services/toast/toast-message.service';
import { ToastMessage } from '../models/ToastMessage';
import { ToastType } from '../enums/ToastType';
import { DataResponse } from '../models/DataResponse';

@Injectable()
export class HttpInterceptorInterceptor implements HttpInterceptor {

  constructor(private toastService: ToastMessageService) { }
  //todo: add error handling here
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const toastMessage: ToastMessage = {
      message: undefined,
      type: ToastType.Success
    };

    return next.handle(request)
      .pipe(
        tap({
          next: (event: HttpEvent<unknown>) => this.handleSuccessMessage(event, toastMessage),
          error: (event: HttpEvent<unknown>) => this.handleErrorMessage(event, toastMessage)
        }),
        finalize(() => {
          if (toastMessage.type == ToastType.Error && toastMessage.message) {
            this.toastService.pushErrorMessage(toastMessage.message!);
          }

          if (toastMessage.type == ToastType.Success && toastMessage.message) {
            this.toastService.pushSuccessMessage(toastMessage.message);
          }

          toastMessage.type = ToastType.Success;
        })
      );
  }

  private handleSuccessMessage(event: HttpEvent<unknown>, toastMessage: ToastMessage): void {
    if (!(event instanceof HttpResponse)) {
      return;
    }

    const response: HttpResponse<DataResponse<any>> = event as HttpResponse<DataResponse<any>>;

    if (!response) {
      return;
    }

    toastMessage.message = response.body!.message;
  }

  private handleErrorMessage(event: HttpEvent<unknown>, toastMessage: ToastMessage): void {
    if (!(event instanceof HttpErrorResponse)) {
      return;
    }

    const response: HttpErrorResponse = event as HttpErrorResponse;
    toastMessage.type = ToastType.Error;

    if (response.status == HttpStatusCode.BadRequest) {
      this.handleBadRequestResponse(response, toastMessage);
      return;
    }

    if (response.status == HttpStatusCode.NotFound) {
      this.handleNotFoundResponse(response, toastMessage);
      return;
    }

    if (response.status >= 500) {
      toastMessage.message = "Something went wrong.";
    }
  }

  handleNotFoundResponse(response: HttpErrorResponse, toastMessage: ToastMessage) {
    if (response.error.message) {
      toastMessage.message = response.error.message;
    }
  }

  private handleBadRequestResponse(response: HttpErrorResponse, toastMessage: ToastMessage) {
    if (response.error.message) {
      toastMessage.message = response.error.message;
    }
  }
}
