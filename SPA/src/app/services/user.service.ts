import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, catchError, of, tap } from 'rxjs';
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
import { ToastMessageService } from './toast/toast-message.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private url: string = environment.apiUrl;
  private redirectionTime: number = 2 * 1000;

  private userId?: string;
  private hasUserSentVerifyRequest: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private isRegistrationSent: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private isUserActivated: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private isResetPasswordRequestSent: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private isDeleteRequestSent: Subject<any> = new Subject();
  private user: BehaviorSubject<UserProfileModel | undefined>;
  public user$: Observable<UserProfileModel | undefined>;
  public isActivated$: Observable<boolean>;
  public isRegistrationSent$: Observable<boolean>;
  public isResetPasswordRequestSent$: Observable<boolean>;
  public hasUserSentVerifyRequest$: Observable<boolean>;
  public isDeleteRequestSent$: Observable<any>;

  constructor(
    private httpClient: HttpClient,
    private jwtService: JwtService,
    private router: Router,
    private toastMessageService: ToastMessageService) {
      this.user = new BehaviorSubject(this.getUser() as UserProfileModel | undefined);
      this.user$ = this.user.asObservable();
      this.isActivated$ = this.isUserActivated.asObservable();
      this.isRegistrationSent$ = this.isRegistrationSent.asObservable();
      this.isResetPasswordRequestSent$ = this.isResetPasswordRequestSent.asObservable();
      this.hasUserSentVerifyRequest$ = this.hasUserSentVerifyRequest.asObservable();
      this.isDeleteRequestSent$ = this.isDeleteRequestSent.asObservable();
    }

  public register(registerRequest: UserRegisterRequest, redirectionRoute: string = "/login"): Observable<DataResponse<UserRegisterResponse>> {
    return this.httpClient.post<DataResponse<UserRegisterResponse>>(this.url + 'register', registerRequest)
      .pipe(
        tap((resp: DataResponse<UserRegisterResponse>) => {
          const response: DataResponse<UserRegisterResponse> = resp as DataResponse<UserRegisterResponse>;

          if (!response.data.isAccountActivationRequired) {
            this.router.navigateByUrl(redirectionRoute);
            return;
          }

          this.isRegistrationSent.next(true);
          this.toastMessageService.pushSuccessMessage(response.message);
        })
      );
  }

  public activateAccount(token: string): Observable<any> {
    return this.httpClient.put(this.url + `account/activate/${token}`, null)
      .pipe(
        tap(() => {
          this.isUserActivated.next(true);
          this.toastMessageService.pushSuccessMessage("Activation sent"); //todo: get response from server
        })
      );
  }

  public login(loginData: UserLoginRequest, redirectionRoute: string = ""): Observable<DataResponse<UserLoginResponse>> {
    return this.httpClient.post<DataResponse<UserLoginResponse>>(this.url + 'login', loginData)
      .pipe(
        tap((response: DataResponse<UserLoginResponse>) => {
          if (response.data.isTwoWayAuth) {
            this.userId = response.data.loginData;
            this.hasUserSentVerifyRequest.next(true);
            return;
          }
          localStorage.setItem("token", response.data.loginData);
          this.user.next(this.getUser());
          this.router.navigateByUrl(redirectionRoute);
          this.toastMessageService.pushSuccessMessage(response.message);
        }),
        catchError((e) => {
          console.error(e); // todo: take error message
          return of();
        })
      );
  }

  public verifyLogin(code: number, redirectionRoute: string | null = null): Observable<DataResponse<string>> {
    const userVerifyLoginRequest: UserVerifyLoginRequest = {
      userId: this.userId as string,
      code: code,
    };

    if (this.userId == undefined) {
      throw new Error('User Id not present');
    }

    return this.httpClient.post<DataResponse<string>>(this.url + 'verifyLogin', userVerifyLoginRequest)
      .pipe(
        tap((response: DataResponse<string>) => {
          localStorage.setItem("token", response.data);
          this.toastMessageService.pushSuccessMessage(response.message);

          this.user.next(this.getUser());
          if (!redirectionRoute) {
            return;
          }

          this.router.navigateByUrl(redirectionRoute);
        })
      );
  }

  private getUser(): UserProfileModel | undefined {
    return this.getUserDataFromToken();
  }

  // todo: try to create an observable to return the user, maybe
  private getUserDataFromToken(): UserProfileModel | undefined {
    const token: string = localStorage.getItem('token') as string;

    if (!token) {
      return undefined;
    }

    const decodedToken: string = this.jwtService.decode(token);
    const payload: any = JSON.parse(decodedToken);

    return JSON.parse(payload.sub) as UserProfileModel;
  }

  public updateUser(userToUpdate: UserProfileModel, redirectionRoute: string | null = null): Observable<DataResponse<string>> {
    const token: string | null = localStorage.getItem("token");
    const headers: HttpHeaders = new HttpHeaders({Authorization: "Bearer " + token});

    return this.httpClient.put<DataResponse<string>>(`${this.url}Profile`, userToUpdate, {headers})
      .pipe(
        tap((response: DataResponse<string>) => {
          this.toastMessageService.pushSuccessMessage(response.message);
          if (!redirectionRoute) {
            return;
          }
          localStorage.setItem("token", response.data);
          this.user.next(this.getUser());
          this.router.navigateByUrl(redirectionRoute);
        })
      );
  }

  public resetPassword(email: string): Observable<any> {
    return this.httpClient.post(this.url + `resetPassword`, `\"${email}\"`, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    })
    .pipe(
      tap(() => this.isResetPasswordRequestSent.next(true))
    );
  }

  public deleteUser(id: string, redirectRoute: string = "/delete"): Observable<any> {
    const token: string | null = localStorage.getItem("token");
    const headers: HttpHeaders = new HttpHeaders({Authorization: "Bearer " + token});

    return this.httpClient.delete(`${this.url}Profile?id=${id}`, {headers})
      .pipe(
        tap(() => {
          this.router.navigateByUrl(redirectRoute);

          setTimeout(() => {
            this.logout();
            this.router.navigateByUrl("");
          }, this.redirectionTime);
        })
      );
  }

  public logout() {
    localStorage.removeItem('token');
    this.user.next(this.getUser());
  }
}
