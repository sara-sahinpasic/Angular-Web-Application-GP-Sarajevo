import { Component, OnInit } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { TicketModel } from 'src/app/models/Admin/Ticket/TicketModel';
import { DataResponse } from 'src/app/models/DataResponse';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { ModalService } from 'src/app/services/modal/modal.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-admin-tickets-page',
  templateUrl: './admin-tickets-page.component.html',
  styleUrls: ['./admin-tickets-page.component.scss']
})
export class AdminTicketsPageComponent implements OnInit{

  protected paginationModel: Pagination = {
    pageSize: 5,
    page: 1,
    totalCount: 0,
  };
  protected filteredTickets: TicketModel[] = [];
  protected tickets: TicketModel[] = [];
  protected editIcon = faEdit;
  protected deleteIcon = faTrash;

  constructor(private ticketService: TicketService, private modalService: ModalService) {}

  ngOnInit() {
    const shouldIncludeInactive: boolean = true;

    this.ticketService.getAllTickets(shouldIncludeInactive)
    .pipe(
      tap(this.loadTickets.bind(this))
    )
    .subscribe();
  }

  loadTickets(response: DataResponse<TicketModel[]>) {
    this.tickets = response.data;
    this.filteredTickets = response.data;
  }

  onPageChange(event) {
    this.paginationModel.page = event;
  }

  showTicketEditModal(ticket: TicketModel) {
    this.modalService.data = ticket;
    this.modalService.adminShowTicketsModal(ticket.name);
  }

  deleteTicket(ticket: TicketModel) {
    this.ticketService.deleteTicket(ticket.id)
      .subscribe(this.reloadPage);
  }

  reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 3000);
  }

  findByTicketName(event: KeyboardEvent) {
    const target: HTMLInputElement = event.target as HTMLInputElement;

    if (!target.value) {
      this.resetFilter();
      return;
    }

    this.filteredTickets = this.tickets.filter(ticket => ticket.name.toLowerCase().indexOf(target.value.toLowerCase()) > -1);
  }

  resetFilter() {
    this.filteredTickets = this.tickets;
  }
}
