import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject, catchError, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserLoginRequest } from '../models/User/UserLoginRequest';
import { UserVerifyLoginRequest } from '../models/User/UserVerifyLoginRequest';
import { UserRegisterRequest } from '../models/User/UserRegisterRequest';
import { DataResponse } from '../models/DataResponse';
import { UserProfileModel } from '../models/User/UserProfileModel';
import { JwtService } from './jwt/jwt.service';
import { Router } from '@angular/router';
import { UserLoginResponse } from '../models/User/UserLoginResponse';
import { UserRegisterResponse } from '../models/User/UserRegisterResponse';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private url: string = environment.apiUrl;
  private userId?: string;
  hasUserSentVerifyRequest: Subject<boolean> = new BehaviorSubject<boolean>(
    false
  );
  isRegistrationSent: Subject<boolean> = new BehaviorSubject<boolean>(false);
  isUserActivated: Subject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(
    private httpClient: HttpClient,
    private jwtService: JwtService,
    private router: Router
  ) {}

  public register(
    registerRequest: UserRegisterRequest,
    redirectionRoute: string = '/login'
  ) {
    this.httpClient
      .post<DataResponse<UserRegisterResponse>>(
        this.url + 'register',
        registerRequest
      )
      .pipe(
        tap((resp: DataResponse<UserRegisterResponse>) => {
          if (resp.data.isAccountActivationRequired) {
            this.isRegistrationSent.next(true);
          }
        })
      )
      .subscribe((resp: DataResponse<UserRegisterResponse | never[]>) => {
        const response: DataResponse<UserRegisterResponse> =
          resp as DataResponse<UserRegisterResponse>;

        if (!response.data.isAccountActivationRequired) {
          this.router.navigateByUrl(redirectionRoute);
        }
      });
  }

  public activateAccount(token: string) {
    this.httpClient
      .put(this.url + `account/activate/${token}`, null)
      .pipe(
        tap(() => {
          this.isUserActivated.next(true);
        })
      )
      .subscribe();
  }

  public login(loginData: UserLoginRequest, redirectionRoute: string = '') {
    this.httpClient
      .post<DataResponse<UserLoginResponse>>(this.url + 'login', loginData)
      .pipe(
        tap((response: DataResponse<UserLoginResponse>) => {
          if (response.data.isTwoWayAuth) {
            this.userId = response.data.loginData;
            this.hasUserSentVerifyRequest.next(true);
            return;
          }
          localStorage.setItem('token', response.data.loginData);
        }),
        catchError((e) => {
          console.error(e); // todo: take error message
          return of([]);
        })
      )
      .subscribe((response: DataResponse<UserLoginResponse> | never[]) => {
        const resp = response as DataResponse<UserLoginResponse>;
        this.router.navigateByUrl(redirectionRoute);
      });
  }

  public verifyLogin(code: number, redirectionRoute: string | null = null) {
    const userVerifyLoginRequest: UserVerifyLoginRequest = {
      userId: this.userId as string,
      code: code,
    };

    if (this.userId == undefined) {
      throw new Error('User Id not present');
    }

    this.httpClient
      .post<DataResponse<string>>(
        this.url + 'verifyLogin',
        userVerifyLoginRequest
      )
      .pipe(
        tap((response: DataResponse<string>) => {
          localStorage.setItem('token', response.data);
        })
      )
      .subscribe(() => {
        if (!redirectionRoute) {
          return;
        }

        this.router.navigateByUrl(redirectionRoute);
      });
  }

  public getUser(): UserProfileModel | undefined {
    return this.getUserDataFromToken();
  }

  private getUserDataFromToken(): UserProfileModel | undefined {
    const token: string = localStorage.getItem('token') as string;

    if (!token) {
      return undefined;
    }

    const decodedToken: string = this.jwtService.decode(token);
    const payload: any = JSON.parse(decodedToken);

    return JSON.parse(payload.sub) as UserProfileModel;
  }

  public updateUser(
    userToUpdate: UserProfileModel,
    redirectionRoute: string | null = null
  ) {
    this.httpClient.put(`${this.url}Profile`, userToUpdate).subscribe(() => {
      if (!redirectionRoute) {
        return;
      }

      this.router.navigateByUrl(redirectionRoute);
    });
  }

  public resetPassword(email: string) {
    this.httpClient
      .post(this.url + `resetPassword`, `\"${email}\"`, {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
      })
      .subscribe();
  }

  public logout() {
    localStorage.removeItem('token');
  }
}
