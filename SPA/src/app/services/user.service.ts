import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserLoginRequest } from '../models/User/UserLoginRequest';
import { UserVerifyLoginRequest } from '../models/User/UserVerifyLoginRequest';
import { UserRegisterRequest } from '../models/User/UserRegisterRequest';
import { UserLoginResponse } from '../models/User/UserLoginResponse';
import { UserToken } from '../models/User/UserToken';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public register(registerRequest: UserRegisterRequest): Observable<string> {
    return this.httpClient.post<string>(this.url + 'register', registerRequest);
  }

  public activateAccount(token: string, userId: string): Observable<string> {
    return this.httpClient.put<string>(this.url + `account/activate/${userId}/${token}`, null);
  }

  public login(loginData: UserLoginRequest): Observable<UserLoginResponse> {
    return this.httpClient.post<UserLoginResponse>(this.url + 'login', loginData);
  }

  public verifyLogin(UserVerifyLoginRequest: UserVerifyLoginRequest): Observable<UserToken> {
    return this.httpClient.post<UserToken>(this.url + 'verifyLogin', UserVerifyLoginRequest);
  }
}
