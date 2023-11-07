import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { environment } from 'src/environments/environment';
import { UserService } from './user.service';
import { UserProfileStatusModel } from 'src/app/models/status/userProfileStatusModel';

@Injectable({
  providedIn: 'root'
})
export class UserStatusService {

  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient, private userService: UserService) { }

  public getAvailableUserStatuses(): Observable<DataResponse<UserStatusDto[]>> {
    return this.httpClient.get<DataResponse<UserStatusDto[]>>(this.apiUrl + 'Profile/Status');
  }

  public getUserStatus(userId: string): Observable<UserProfileStatusModel> {
    return this.httpClient.get<DataResponse<UserProfileStatusModel>>(`${this.apiUrl}Profile/Status/Get/${userId}`)
      .pipe(
        map((response: DataResponse<UserProfileStatusModel>) => response.data)
      );
  }
}
