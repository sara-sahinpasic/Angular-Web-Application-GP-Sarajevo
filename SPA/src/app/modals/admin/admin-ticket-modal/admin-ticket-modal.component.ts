import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TicketDto } from 'src/app/models/Admin/Ticket/TicketDto';
import { AdminTicketService } from 'src/app/services/admin/ticket/admin-ticket.service';

@Component({
  selector: 'app-admin-ticket-modal',
  templateUrl: './admin-ticket-modal.component.html',
  styleUrls: ['./admin-ticket-modal.component.scss'],
})
export class AdminTicketModalComponent implements OnInit {
  constructor(
    protected ticketService: AdminTicketService,
    protected formBuilder: FormBuilder
  ) {}
  ngOnInit(): void {
    this.inizializeValidators();
  }
  protected ticketModel: TicketDto = {};
  protected registrationForm!: FormGroup;

  addTicket() {
    this.registrationForm.markAllAsTouched();
    if (this.registrationForm.valid) {
      this.ticketService.postNewTicket(this.ticketModel).subscribe(() => {
        setTimeout(function () {
          window.location.reload();
        }, 3000);
      });
    }
  }
  inizializeValidators() {
    this.registrationForm = this.formBuilder.group({
      ticketName: ['', Validators.required],
      price: ['', Validators.required],
    });
  }
}
