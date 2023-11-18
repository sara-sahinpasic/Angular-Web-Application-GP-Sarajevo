import { Component } from '@angular/core';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-company-page',
  templateUrl: './admin-company-page.component.html',
  styleUrls: ['./admin-company-page.component.scss'],
})
export class AdminCompanyPageComponent {
  constructor(private modalService: ModalService) {}

  protected adminShowNewsModal() {
    this.modalService.adminShowNewsModal();
  }

  protected adminShowUsersModal() {
    this.modalService.adminShowUsersModal();
  }

  protected adminShowTicketsModal() {
    this.modalService.adminShowTicketsModal();
  }

  protected adminShowVehiclesModal() {
    this.modalService.adminShowVehiclesModal();
  }

  protected adminShowStationsModal() {
    this.modalService.adminShowStationsModal();
  }

  protected showRouteCreationModal() {
    this.modalService.adminShowCreateRouteModal();
  }

  protected showHolidayCreationModal() {
    this.modalService.adminShowHolidayModal();
  }
}
