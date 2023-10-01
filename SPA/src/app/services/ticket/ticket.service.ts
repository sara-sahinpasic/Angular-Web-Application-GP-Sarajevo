import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/InvalidArgumentException';
import { TicketDto } from 'src/app/models/Admin/Ticket/TicketDto';
import { TicketModel } from 'src/app/models/Admin/Ticket/TicketModel';
import { DataResponse } from 'src/app/models/DataResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  public getAllTickets(shouldIncludeInactive: boolean = false): Observable<DataResponse<TicketModel[]>> {
    return this.httpClient.get<DataResponse<TicketModel[]>>(`${this.url}Ticket/All?includeInactive=${shouldIncludeInactive}`);
  }

  public postNewTicket(ticketModel: TicketDto): Observable<DataResponse<TicketDto[]>> {
    return this.httpClient.post<DataResponse<TicketDto[]>>(`${this.url}Ticket`,ticketModel);
  }

  public editTicket(ticket: TicketModel): Observable<DataResponse<any>> {
    if (!ticket) {
      throw new InvalidArgumentException(["The ticket argument cannot be null"]);
    }

    return this.httpClient.put<DataResponse<any>>(`${this.url}Admin/Ticket/Edit/${ticket.id}`, ticket);
  }

  public deleteTicket(ticketId: string): Observable<DataResponse<any>> {
    return this.httpClient.delete<DataResponse<any>>(`${this.url}Admin/Ticket/Delete/${ticketId}`);
  }
}
