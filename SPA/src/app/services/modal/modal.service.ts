import { Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  public showRequestModalState: boolean = false;
  public showReviewModalState: boolean = false;
  public showNewsModalState: boolean = false;
  public data: any;
  private modalTitle: string = '';
  private modalCloseBtn?: HTMLButtonElement;

  constructor() {}

  setCloseBtn(btn: HTMLButtonElement) {
    this.modalCloseBtn = btn;
  }

  showRequestModal() {
    this.showRequestModalState = true;
  }

  showReviewModal() {
    this.showReviewModalState = true;
  }

  showNewsModal() {
    this.showNewsModalState = true;
  }

  setModalTitle(title: string) {
    this.modalTitle = title;
  }

  closeModal() {
    this.showRequestModalState = false;
    this.showReviewModalState = false;
    this.showNewsModalState = false;
    this.modalTitle = '';
    this.data = null;
    this.modalCloseBtn?.click();
  }

  getModalTitle(): string {
    return this.modalTitle;
  }
}
