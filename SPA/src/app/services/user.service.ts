import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserRegisterRequest } from '../models/UserRegisterRequest';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public register(registerRequest: UserRegisterRequest): Observable<string> {
    return this.httpClient.post<string>(this.url + 'register', registerRequest);
  }

  public activateAccount(token: string): Observable<string> {
    return this.httpClient.put<string>(this.url + `account/activate/${token}`, null);
  }
}
