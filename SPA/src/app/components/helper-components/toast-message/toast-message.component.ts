import { Component, OnInit } from '@angular/core';
import { ToastType } from 'src/app/enums/ToastType';
import { ToastMessage } from 'src/app/models/ToastMessage';
import { ToastMessageService } from 'src/app/services/toast/toast-message.service';

@Component({
  selector: 'app-toast-message',
  templateUrl: './toast-message.component.html',
  styleUrls: ['./toast-message.component.scss']
})
export class ToastMessageComponent implements OnInit {

  protected readonly ToastType: typeof ToastType = ToastType;
  protected messagesQue: ToastMessage[] = [];
  protected toastMessageExpirationSeconds: number = 4;

  constructor(public toastMessageService: ToastMessageService) { }

  ngOnInit() {
    this.toastMessageService.message$.subscribe((value: ToastMessage) => {
      this.messagesQue.push(value);
      setTimeout(() => {
        this.messagesQue.shift();
      }, this.toastMessageExpirationSeconds * 1000);
    });
  }
}
