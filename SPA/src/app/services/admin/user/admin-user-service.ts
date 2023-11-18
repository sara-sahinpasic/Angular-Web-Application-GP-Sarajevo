import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserRoleDto } from 'src/app/models/admin/user/userRoleDto';
import { DataResponse } from 'src/app/models/dataResponse';
import { Observable } from 'rxjs';
import { UserGetDto } from 'src/app/models/admin/user/userGetDto';
import { UserCreateModel } from 'src/app/models/admin/user/userCreateModel';

@Injectable({
  providedIn: 'root',
})
export class AdminUserService {
  private apiUrl: string = environment.apiUrl;

  constructor(protected httpClient: HttpClient) {}

  public getAllRoles(): Observable<DataResponse<UserRoleDto[]>> {
    return this.httpClient.get<DataResponse<UserRoleDto[]>>(
      this.apiUrl + 'Admin/User/Roles/All'
    );
  }

  public createUser(user: UserCreateModel): Observable<DataResponse<UserCreateModel[]>> {
    return this.httpClient.post<DataResponse<UserCreateModel[]>>(
      this.apiUrl + 'Admin/User/Create',
      user
    );
  }

  public getAllUsers(): Observable<DataResponse<UserGetDto[]>> {
    return this.httpClient.get<DataResponse<UserGetDto[]>>(
      this.apiUrl + 'Admin/User/All'
    );
  }

  public getUserById(id: string) {
    return this.httpClient.get(`${this.apiUrl}Admin/User/Get/${id}`);
  }
}
