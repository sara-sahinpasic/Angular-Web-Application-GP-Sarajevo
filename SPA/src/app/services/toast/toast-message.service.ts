import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ToastType } from 'src/app/enums/ToastType';
import { ToastMessage } from 'src/app/models/ToastMessage';

@Injectable({
  providedIn: 'root'
})
export class ToastMessageService {

  private messageSubject: Subject<ToastMessage> = new Subject();
  public message$: Observable<ToastMessage>;

  constructor() {
    this.message$ = this.messageSubject.asObservable();
  }

  public pushErrorMessage(message: string) {
    const toastMessage: ToastMessage = this.createMessage(message, ToastType.Error);

    this.pushMessage(toastMessage);
  }

  public pushSuccessMessage(message: string) {
    const toastMessage: ToastMessage = this.createMessage(message, ToastType.Success);

    this.pushMessage(toastMessage);
  }

  private pushMessage(message: ToastMessage) {
    this.messageSubject.next(message);
  }

  private createMessage(message: string, type: ToastType): ToastMessage {
    return {
      message,
      type
    };
  }
}
