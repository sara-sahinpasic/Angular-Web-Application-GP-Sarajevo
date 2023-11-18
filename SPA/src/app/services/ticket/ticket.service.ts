import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/invalidArgumentException';
import { TicketDto } from 'src/app/models/admin/ticket/ticketDto';
import { TicketModel } from 'src/app/models/admin/ticket/ticketModel';
import { DataResponse } from 'src/app/models/dataResponse';
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
    return this.httpClient.post<DataResponse<TicketDto[]>>(`${this.url}Admin/Ticket/Add`,ticketModel);
  }

  public editTicket(ticket: TicketModel): Observable<DataResponse<any>> {
    if (!ticket) {
      throw new InvalidArgumentException(['The ticket argument cannot be null']);
    }

    return this.httpClient.put<DataResponse<any>>(`${this.url}Admin/Ticket/Edit/${ticket.id}`, ticket);
  }

  public deleteTicket(ticketId: string): Observable<DataResponse<any>> {
    return this.httpClient.delete<DataResponse<any>>(`${this.url}Admin/Ticket/Delete/${ticketId}`);
  }
}
