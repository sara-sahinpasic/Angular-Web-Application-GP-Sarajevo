import { Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ModalService implements OnInit {
  public showRequestModalState: boolean = false;
  public showPurchaseHistoryModalState: boolean = false;
  private showModalTitle: string = '';

  constructor() {}
  ngOnInit(): void {}

  showRequestModal() {
    this.showRequestModalState = true;
    this.showModalTitle = 'Zahtjev';
  }
  showPurchaseHistoryModal() {
    this.showPurchaseHistoryModalState = true;
    this.showModalTitle = 'Historija kupovina';
  }
  closeModal() {
    this.showRequestModalState = false;
    this.showPurchaseHistoryModalState = false;
    this.showModalTitle = '';
  }
  getModalTitle(): string {
    return this.showModalTitle;
  }
}
