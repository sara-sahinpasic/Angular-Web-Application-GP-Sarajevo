import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { RequestDto } from 'src/app/models/request/requestDto';
import { environment } from 'src/environments/environment';
import { UserService } from '../user/user.service';
import { DataResponse } from 'src/app/models/dataResponse';
import { ApproveRequestModel } from 'src/app/models/request/approveRequestModel';
import { RejectRequestModel } from 'src/app/models/request/rejectRequestModel';
import { RequestModel } from 'src/app/models/request/requestModel';

@Injectable({
  providedIn: 'root',
})
export class SpecialRequestService {
  private url = environment.apiUrl;

  constructor(private httpClient: HttpClient, private userService: UserService) {}

  public requestDiscount(requestDiscountData: RequestDto): Observable<HttpEvent<DataResponse<string>>> {
    const formData = this.getFormDataFromObject(requestDiscountData);

    return this.httpClient.post<DataResponse<string>>(`${this.url}User/Request/Send`, formData, {
      reportProgress: true,
      observe: 'events'
    });
  }

  private getFormDataFromObject(object: any): FormData {
    const formData = new FormData();
    Object.keys(object).forEach(key => formData.append(key, object[key]));

    return formData;
  }

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
