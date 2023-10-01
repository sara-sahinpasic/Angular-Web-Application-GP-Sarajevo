import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TicketDto } from 'src/app/models/Admin/Ticket/TicketDto';
import { TicketModel } from 'src/app/models/Admin/Ticket/TicketModel';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-admin-ticket-modal',
  templateUrl: './admin-ticket-modal.component.html',
  styleUrls: ['./admin-ticket-modal.component.scss'],
})
export class AdminTicketModalComponent implements OnInit {

  @Input() ticket?: TicketModel;
  protected ticketModel: TicketDto = {};
  protected registrationForm!: FormGroup;

  constructor(
    protected ticketService: TicketService,
    protected formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.inizializeValidators();

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

  addTicket() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.ticketService.postNewTicket(this.ticketModel).subscribe(this.reloadPage);
    }
  }

  private reloadPage() {
    setTimeout(function () {
      window.location.reload();
    }, 3000);
  }

  editTicket(ticket: TicketDto) {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.invalid) {
      return;
    }

    this.ticketService.editTicket(ticket as TicketModel)
      .subscribe(this.reloadPage);
  }

  inizializeValidators() {
    this.registrationForm = this.formBuilder.group({
      ticketName: ['', Validators.required],
      price: ['', Validators.required],
    });
  }
}
