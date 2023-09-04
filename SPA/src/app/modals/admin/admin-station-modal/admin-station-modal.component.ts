import { Component } from '@angular/core';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-station-modal',
  templateUrl: './admin-station-modal.component.html',
  styleUrls: ['./admin-station-modal.component.scss'],
})
export class AdminStationModalComponent {

  constructor(private modalService: ModalService) {}

  deleteStation() {}
  adminShowNewStationsModal() {
    //ToDo: when pressing this button, it is necessary to turn off the current modal
    this.modalService.adminShowNewStationsModal();
  }
}
