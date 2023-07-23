import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
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
    const token: string | null = this.userService.getUserJwtToken();
    const headers: HttpHeaders = new HttpHeaders({
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.post<DataResponse<string>>(`${this.baseApiUrl}Request/UploadFile`, formData, {
      reportProgress: true,
      headers,
      observe: 'events'
    });
  }

  private getFormDataFromObject(object: any): FormData {
    const formData = new FormData();
    Object.keys(object).forEach(key => formData.append(key, object[key]));

    return formData;
  }
}
