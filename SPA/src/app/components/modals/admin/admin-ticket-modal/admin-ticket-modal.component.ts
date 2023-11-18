import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TicketDto } from 'src/app/models/admin/ticket/ticketDto';
import { TicketModel } from 'src/app/models/admin/ticket/ticketModel';
import { ModalService } from 'src/app/services/modal/modal.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-admin-ticket-modal',
  templateUrl: './admin-ticket-modal.component.html',
  styleUrls: ['./admin-ticket-modal.component.scss'],
})
export class AdminTicketModalComponent implements OnInit {
  @Input() ticket?: TicketModel;
  protected ticketModel: TicketDto = {};
  protected registrationForm: FormGroup = {} as FormGroup;

  constructor(
    private ticketService: TicketService,
    private formBuilder: FormBuilder,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.initializeFrom();

    if (this.ticket) {
      this.mapTicketToTicketModel();
    }
  }

  private mapTicketToTicketModel() {
    this.ticketModel.active = this.ticket?.active;
    this.ticketModel.name = this.ticket?.name;
    this.ticketModel.price = this.ticket?.price;
    this.ticketModel.id = this.ticket?.id;
  }

  protected addTicket() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.ticketService.postNewTicket(this.ticketModel).subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  protected editTicket(ticket: TicketDto) {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.invalid) {
      return;
    }

    this.ticketService
      .editTicket(ticket as TicketModel)
      .subscribe(this.reloadPage);
  }

  private initializeFrom() {
    this.registrationForm = this.formBuilder.group({
      ticketName: ['', Validators.required],
      price: ['', Validators.required],
    });
  }
}
