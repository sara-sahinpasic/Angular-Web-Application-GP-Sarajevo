import { Component } from '@angular/core';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-company-page',
  templateUrl: './admin-company-page.component.html',
  styleUrls: ['./admin-company-page.component.scss'],
})
export class AdminCompanyPageComponent {
  constructor(private modalService: ModalService) {}

  adminShowNewsModal() {
    this.modalService.adminShowNewsModal();
  }

  adminShowUsersModal() {
    this.modalService.adminShowUsersModal();
  }

  adminShowTicketsModal() {
    this.modalService.adminShowTicketsModal();
  }

  adminShowVehiclesModal() {
    this.modalService.adminShowVehiclesModal();
  }

  adminShowStationsModal() {
    this.modalService.adminShowStationsModal();
  }

  showRouteCreationModal() {
    this.modalService.adminShowCreateRouteModal();
  }
}
