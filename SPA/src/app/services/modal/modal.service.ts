import { Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ModalService implements OnInit {
  public showRequestModalState: boolean = false;
  public showReviewModalState: boolean = false;
  public showNewsModalState: boolean = false;

  private showModalTitle: string = '';

  public data: any;

  constructor() {}

  ngOnInit(): void {}

  showRequestModal() {
    this.showRequestModalState = true;
    this.showModalTitle = 'Zahtjev';
  }
  showReviewModal() {
    this.showReviewModalState = true;
    this.showModalTitle = '';
  }
  showNewsModal() {
    this.showNewsModalState = true;
    this.showModalTitle = '';
  }

  closeModal() {
    this.showRequestModalState = false;
    this.showReviewModalState = false;
    this.showNewsModalState = false;
    this.showModalTitle = '';
    this.data = null;
  }

  getModalTitle(): string {
    return this.showModalTitle;
  }
}
