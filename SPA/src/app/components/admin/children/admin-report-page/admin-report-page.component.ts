import { Component } from '@angular/core';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-report-page',
  templateUrl: './admin-report-page.component.html',
  styleUrls: ['./admin-report-page.component.scss']
})
export class AdminReportPageComponent {


  constructor(private modalService: ModalService){}
  showModal(){
    this.modalService.showRequestModal();
  }

}
