import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { TicketModel } from 'src/app/models/card-type/CardTypeModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  public getAllTickets(): Observable<DataResponse<TicketModel[]>> {
    return this.httpClient.get<DataResponse<TicketModel[]>>(`${this.url}Ticket/All`);
  }
}
