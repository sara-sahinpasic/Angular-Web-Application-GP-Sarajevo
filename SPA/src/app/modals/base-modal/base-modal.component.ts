import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-base-modal',
  templateUrl: './base-modal.component.html',
  styleUrls: ['./base-modal.component.scss'],
})
export class BaseModalComponent implements AfterViewInit {

  @ViewChild("closeModalBtn") closeBtnElementRef!: ElementRef<HTMLButtonElement>;

  constructor(protected modalService: ModalService) {}

  ngAfterViewInit(): void {
    const closeBtn: HTMLButtonElement = this.closeBtnElementRef.nativeElement;
    this.modalService.setCloseBtn(closeBtn);
  }
}
