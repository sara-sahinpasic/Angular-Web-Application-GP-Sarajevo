import { Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ModalService implements OnInit {
  public showRequestModalState: boolean = false;
  public showReviewModalState: boolean = false;
  private showModalTitle: string = '';

  constructor() {}

  ngOnInit(): void {}

  showRequestModal() {
    this.showRequestModalState = true;
    this.showModalTitle = 'Zahtjev';
  }
  showReviewModal(){
    this.showReviewModalState=true;
    this.showModalTitle='';
  }

  closeModal() {
    this.showRequestModalState = false;
    this.showReviewModalState = false;
    this.showModalTitle = '';
  }

  getModalTitle(): string {
    return this.showModalTitle;
  }
}
