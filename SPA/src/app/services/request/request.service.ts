import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { ApproveRequestModel } from 'src/app/models/Request/approveRequestModel';
import { RejectRequestModel } from 'src/app/models/Request/rejectRequestModel';
import { RequestModel } from 'src/app/models/Request/requestModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RequestService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  public getRequestForUser(userId: string): Observable<RequestModel> {
    return this.httpClient.get<DataResponse<RequestModel>>(`${this.url}User/Request/Get/${userId}`)
      .pipe(
        map((response: DataResponse<RequestModel>) => response.data)
      );
  }

  public approveRequest(requestId: string, approveRequestModel: ApproveRequestModel): Observable<unknown> {
    return this.httpClient.put<DataResponse<unknown>>(`${this.url}User/Request/Approve/${requestId}`, approveRequestModel);
  }

  public rejectRequest(requestId: string, rejectRequestModel: RejectRequestModel): Observable<unknown> {
    return this.httpClient.put<DataResponse<unknown>>(`${this.url}User/Request/Reject/${requestId}`, rejectRequestModel);
  }
}
