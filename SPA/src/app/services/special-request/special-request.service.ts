import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { RequestDto } from 'src/app/models/Request/RequestDto';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { environment } from 'src/environments/environment';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root',
})
export class SpecialRequestService {
  private baseApiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient, private userService: UserService) {}

  requestDiscount(requestDiscountData: RequestDto): Observable<HttpEvent<DataResponse<string>>> {
    const formData = this.getFormDataFromObject(requestDiscountData);

    return this.httpClient.post<DataResponse<string>>(`${this.baseApiUrl}Request/SendRequest`, formData, {
      reportProgress: true,
      observe: 'events'
    });
  }

  private getFormDataFromObject(object: any): FormData {
    const formData = new FormData();
    Object.keys(object).forEach(key => formData.append(key, object[key]));

    return formData;
  }
}
