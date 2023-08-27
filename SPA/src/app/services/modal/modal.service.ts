import { Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ModalService implements OnInit {
  public showRequestModalState: boolean = false;
  public showReviewModalState: boolean = false;
  public showNewsModalState: boolean = false;

  private modalTitle: string = '';

  public data: any;

  constructor() {}

  ngOnInit(): void {}

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
  }

  getModalTitle(): string {
    return this.modalTitle;
  }
}
