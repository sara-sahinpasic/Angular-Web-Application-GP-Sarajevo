import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, firstValueFrom, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserLoginRequest } from '../models/User/UserLoginRequest';
import { UserVerifyLoginRequest } from '../models/User/UserVerifyLoginRequest';
import { UserRegisterRequest } from '../models/User/UserRegisterRequest';
import { UserLoginResponse } from '../models/User/UserLoginResponse';
import { UserToken } from '../models/User/UserToken';
import { DataResponse } from '../models/DataResponse';
import { UserProfileModel } from '../models/User/UserProfileModel';
import { JwtService } from './jwt/jwt.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url: string = environment.apiUrl;
  private user?: UserProfileModel;

  constructor(private httpClient: HttpClient, private jwtService: JwtService) {}

  public register(registerRequest: UserRegisterRequest): Observable<DataResponse> {
    return this.httpClient.post<DataResponse>(this.url + 'register', registerRequest);
  }

  public activateAccount(token: string, userId: string): Observable<DataResponse> {
    return this.httpClient.put<DataResponse>(this.url + `account/activate/${userId}/${token}`, null);
  }

  public async login(loginData: UserLoginRequest): Promise<string | undefined> {
    let userId: string | undefined;
    const observable: Observable<DataResponse | never[]> = this.httpClient.post<DataResponse>(this.url + 'login', loginData)
      .pipe(
        tap((response: DataResponse) => {
          userId = response.data as string
        }),
        catchError(e => {
          console.error(e); // todo: take error message
          return of([]);
        })
      );

    await firstValueFrom(observable);
    return userId;
  }

  public async verifyLogin(userVerifyLoginRequest: UserVerifyLoginRequest): Promise<boolean> {
    let isSuccesfull: boolean = false;

    const observable: Observable<DataResponse> = this.httpClient.post<DataResponse>(this.url + 'verifyLogin', userVerifyLoginRequest)
      .pipe(
        tap((response: DataResponse) => {
          localStorage.setItem("token", response.data as string);
          isSuccesfull = true;
        })
      );

      await firstValueFrom(observable);

      return isSuccesfull;
  }

  public getUser(): UserProfileModel | undefined {
    return this.getUserDataFromToken();
  }

  private getUserDataFromToken(): UserProfileModel {
    const token: string = localStorage.getItem("token") as string;
    const decodedToken: string = this.jwtService.decode(token);
    const payload: any = JSON.parse(decodedToken);

    return JSON.parse(payload.sub) as UserProfileModel;
  }
}
