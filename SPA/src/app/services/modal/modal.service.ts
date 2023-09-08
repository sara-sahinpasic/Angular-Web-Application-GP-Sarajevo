import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  //user:
  public showRequestModalState: boolean = false;
  public showReviewModalState: boolean = false;
  public showNewsModalState: boolean = false;
  // admin:
  public showAdminNewsModalState: boolean = false;
  public showAdminUsersModalState: boolean = false;
  public showAdminTicketsModalState: boolean = false;
  public showAdminVehiclesModalState: boolean = false;
  public showAdminStationsModalState: boolean = false;
  public showAdminNewStationsModalState: boolean = false;
  public showAdminUpdateUserModalState: boolean = false;

  public data: any;
  private modalTitle: string = '';
  private modalCloseBtn?: HTMLButtonElement;

  constructor() {}

  setCloseBtn(btn: HTMLButtonElement) {
    this.modalCloseBtn = btn;
  }
  //admin:
  adminShowUpdateUserModal() {
    this.showAdminUpdateUserModalState = true;
    this.modalTitle = 'Izmjena podataka';
  }
  adminShowNewsModal() {
    this.showAdminNewsModalState = true;
    this.modalTitle = 'Nova obavijest';
  }
  adminShowUsersModal() {
    this.modalTitle = 'Novi korisnik';
    this.showAdminUsersModalState = true;
  }
  adminShowTicketsModal() {
    this.modalTitle = 'Nova karta';
    this.showAdminTicketsModalState = true;
  }
  adminShowVehiclesModal() {
    this.modalTitle = 'Novo vozilo';
    this.showAdminVehiclesModalState = true;
  }
  adminShowStationsModal() {
    this.modalTitle = 'Stanica';
    this.showAdminStationsModalState = true;
  }
  adminShowNewStationsModal() {
    this.showAdminNewStationsModalState = true;
  }
  //user:
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
    //admin:
    this.showAdminUpdateUserModalState=false;
    this.showAdminNewsModalState = false;
    this.showAdminUsersModalState = false;
    this.showAdminTicketsModalState = false;
    this.showAdminVehiclesModalState = false;
    this.showAdminStationsModalState = false;
    this.showAdminNewStationsModalState = false;

    //user:
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
