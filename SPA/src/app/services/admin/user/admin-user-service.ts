import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserRoleDto } from 'src/app/models/Admin/User/UserRoleDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { Observable, tap } from 'rxjs';
import { UserGetDto } from 'src/app/models/Admin/User/UserGetDto';
import { UserCreateModel } from 'src/app/models/Admin/User/UserCreateModel';

@Injectable({
  providedIn: 'root',
})
export class AdminUserService {
  constructor(protected httpClient: HttpClient) {}

  private apiUrl: string = environment.apiUrl;

  getAllRoles(): Observable<DataResponse<UserRoleDto[]>> {
    return this.httpClient.get<DataResponse<UserRoleDto[]>>(
      this.apiUrl + 'Roles'
    );
  }

  createUser(
    user: UserCreateModel
  ): Observable<DataResponse<UserCreateModel[]>> {
    return this.httpClient.post<DataResponse<UserCreateModel[]>>(
      this.apiUrl + 'CreateUser',
      user
    );
  }

  getAllUsers(): Observable<DataResponse<UserGetDto[]>> {
    return this.httpClient.get<DataResponse<UserGetDto[]>>(
      this.apiUrl + 'Users'
    );
  }

  getUserById(id: string) {
    return this.httpClient.get(`${this.apiUrl}UserById?id=${id}`);
  }
}
