import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { environment } from 'src/environments/environment';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class UserStatusService {

  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient, private userService: UserService) { }

  public getAvailableUserStatuses(): Observable<DataResponse<UserStatusDto[]>> {
    const headers: HttpHeaders = new HttpHeaders({
      Authorization: 'Bearer ' +  this.userService.getUserJwtToken()
    });

    return this.httpClient.get<DataResponse<UserStatusDto[]>>(this.apiUrl + 'Profile/Status', { headers });
  }
}
