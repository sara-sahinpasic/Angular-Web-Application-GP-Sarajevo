import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, map, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserLoginRequest } from '../../models/user/userLoginRequest';
import { UserVerifyLoginRequest } from '../../models/user/userVerifyLoginRequest';
import { UserRegisterRequest } from '../../models/user/userRegisterRequest';
import { DataResponse } from '../../models/dataResponse';
import { UserProfileModel } from '../../models/user/userProfileModel';
import { JwtService } from '../jwt/jwt.service';
import { Router } from '@angular/router';
import { UserLoginResponse } from '../../models/user/userLoginResponse';
import { UserRegisterResponse } from '../../models/user/userRegisterResponse';
import { UserEditProfileModel } from 'src/app/models/user/userEditProfileModel';
import { UserTokenData } from 'src/app/models/user/userToken';
import { Role } from 'src/app/models/roles/role';

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
  private isDeleteRequestSent: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private user: BehaviorSubject<UserProfileModel | undefined>;
  public user$: Observable<UserProfileModel | undefined>;
  public isActivated$: Observable<boolean>;
  public isRegistrationSent$: Observable<boolean>;
  public isResetPasswordRequestSent$: Observable<boolean>;
  public hasUserSentVerifyRequest$: Observable<boolean>;
  public isDeleteRequestSent$: Observable<any>;

  constructor(private httpClient: HttpClient, private jwtService: JwtService, private router: Router) {
    this.user = new BehaviorSubject(this.getUser() as UserProfileModel | undefined);
    this.user$ = this.user.asObservable();
    this.isActivated$ = this.isUserActivated.asObservable();
    this.isRegistrationSent$ = this.isRegistrationSent.asObservable();
    this.isResetPasswordRequestSent$ = this.isResetPasswordRequestSent.asObservable();
    this.hasUserSentVerifyRequest$ = this.hasUserSentVerifyRequest.asObservable();
    this.isDeleteRequestSent$ = this.isDeleteRequestSent.asObservable();
  }

  // ===================== Authentication =================================
  public register(registerRequest: UserRegisterRequest, redirectionRoute: string | null = null): Observable<DataResponse<UserRegisterResponse>> {
    return this.httpClient
      .post<DataResponse<UserRegisterResponse>>(
        this.url + 'Authentication/Register',
        registerRequest
      )
      .pipe(
        tap((resp: DataResponse<UserRegisterResponse>) => {
          const response: DataResponse<UserRegisterResponse> = resp as DataResponse<UserRegisterResponse>;

          if (!response.data.isAccountActivationRequired) {
            if (redirectionRoute) {
              this.router.navigateByUrl(redirectionRoute);
            }
            return;
          }

          this.isRegistrationSent.next(true);
        })
      );
  }

  public activateAccount(token: string): Observable<any> {
    return this.httpClient
      .put(this.url + `Authentication/Account/Activate/${token}`, null)
      .pipe(
        tap(() => {
          this.isUserActivated.next(true);
        })
      );
  }

  public login(loginData: UserLoginRequest, redirectionRoute: string | null = ''): Observable<DataResponse<UserLoginResponse>> {
    return this.httpClient
      .post<DataResponse<UserLoginResponse>>(this.url + 'Authentication/Login', loginData)
      .pipe(
        tap((response: DataResponse<UserLoginResponse>) => {
          if (response.data.isTwoWayAuth) {
            this.userId = response.data.loginData as string;
            this.hasUserSentVerifyRequest.next(true);
            return;
          }

          const userTokenData: UserTokenData = (response.data.loginData) as UserTokenData;
          localStorage.setItem('token', userTokenData.access_token);

          const user: UserProfileModel = this.getUser() as UserProfileModel;

          this.user.next(user);

          if (user.role.toLowerCase() === 'admin') {
            this.router.navigateByUrl('/admin');
            return;
          }

          if (user.role.toLowerCase() === "driver") {
            this.router.navigateByUrl("/driver");
            return;
          }

          if (redirectionRoute != null) {
            this.router.navigateByUrl(redirectionRoute);
          }
        })
      );
  }

  public verifyLogin(code: number, redirectionRoute: string | null = null): Observable<DataResponse<UserLoginResponse>> {
    const userVerifyLoginRequest: UserVerifyLoginRequest = {
      userId: this.userId as string,
      code: code,
    };

    if (!this.userId) {
      throw new Error('User Id not present');
    }

    return this.httpClient.post<DataResponse<UserLoginResponse>>(this.url + 'Authentication/VerifyLogin', userVerifyLoginRequest)
      .pipe(
        tap((response: DataResponse<UserLoginResponse>) => {
          localStorage.setItem('token', (response.data.loginData as UserTokenData).access_token);

          this.user.next(this.getUser());
          this.hasUserSentVerifyRequest.next(false);

          const user: UserProfileModel = this.getUser() as UserProfileModel;

          if (user.role.toLowerCase() === 'admin') {
            this.router.navigateByUrl('/admin');
            return;
          }

          if (redirectionRoute != null) {
            this.router.navigateByUrl(redirectionRoute);
          }
        })
      );
  }

  public resetPassword(email: string): Observable<any> {
    return this.httpClient
      .post(this.url + `Authentication/ResetPassword`, `\"${email}\"`, {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
      })
      .pipe(tap(() => this.isResetPasswordRequestSent.next(true)));
  }

  public logout() {
    localStorage.removeItem('token');
    this.user.next(this.getUser());
    this.router.navigateByUrl('');
  }

  public getUserJwtToken(): string | null {
    return localStorage.getItem('token');
  }

  public getUserRole(): string | null {
    const user: UserProfileModel | undefined = this.getUser();

    if (!user) {
      return null;
    }

    return user.role;
  }

  public isLoggedIn(): boolean {
    return !!this.getUser();
  }

  public isUserInRole(role: Role): boolean {
    return !!this.getUserRole() && role.name.toLocaleLowerCase() === this.getUserRole()?.toLocaleLowerCase();
  }
  // ===================== End =================================

  private getUser(): UserProfileModel | undefined {
    return this.getUserDataFromToken();
  }

  private getUserDataFromToken(): UserProfileModel | undefined {
    const token: string = localStorage.getItem('token') as string;

    if (!token) {
      return undefined;
    }

    const decodedToken: string = this.jwtService.decode(token);
    const payload: any = JSON.parse(decodedToken);

    try {
      return payload as UserProfileModel;
    } catch (error) {
      return undefined;
    }
  }

  public updateUser(userToUpdate: UserEditProfileModel, redirectionRoute: string | null = null): Observable<DataResponse<UserTokenData>> {
    const formData: FormData = this.getFormDataFromObject(userToUpdate);

    return this.httpClient
      .put<DataResponse<UserTokenData>>(`${this.url}User/Update`, formData)
      .pipe(
        tap((response: DataResponse<UserTokenData>) => {
          if (response.data) {
            localStorage.setItem('token', response.data.access_token);
            this.user.next(this.getUser());
          }

          if (!redirectionRoute) {
            return;
          }

          this.router.navigateByUrl(redirectionRoute);
        })
      );
  }

  private getFormDataFromObject(object: any): FormData {
    const formData = new FormData();
    Object.keys(object).forEach((key) => formData.append(key, object[key]));

    return formData;
  }

  public deleteUser(id: string, redirectRoute: string | null = '/delete'): Observable<DataResponse<string>> {
    return this.httpClient
      .delete<DataResponse<string>>(`${this.url}User/Delete?id=${id}`)
      .pipe(
        tap(() => {
          if (!redirectRoute) {
            return;
          }
          this.isDeleteRequestSent.next(true);
          this.router.navigateByUrl(redirectRoute);

          setTimeout(() => {
            this.logout();
            this.router.navigateByUrl('');
            this.isDeleteRequestSent.next(false);
          }, this.redirectionTime);
        })
      );
  }

  public getUserDiscount(): Observable<number> {
    const userId: string | undefined = this.getUser()?.id;

    return this.httpClient.get<DataResponse<number>>(`${this.url}User/Discount/Get/${userId}`)
      .pipe(
        map((response: DataResponse<number>) => response.data)
      );
  }

  public getProfileImage(): Observable<DataResponse<string>> {
    let user: UserProfileModel;
    user = this.getUser() as UserProfileModel;

    return this.httpClient.get<DataResponse<string>>(
      `${this.url}User/Image/${user.id}`
    );
  }
}
