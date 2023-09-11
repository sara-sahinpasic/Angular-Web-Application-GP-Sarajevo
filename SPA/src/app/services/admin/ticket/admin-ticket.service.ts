import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TicketDto } from 'src/app/models/Admin/Ticket/TicketDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AdminTicketService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public postNewTicket(
    ticketModel: TicketDto
  ): Observable<DataResponse<TicketDto[]>> {
    return this.httpClient.post<DataResponse<TicketDto[]>>(
      `${this.apiUrl}Ticket`,
      ticketModel
    );
  }
}
