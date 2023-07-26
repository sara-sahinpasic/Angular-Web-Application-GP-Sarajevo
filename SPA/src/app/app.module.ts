import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoadingInterceptor } from './http-interceptors/loading.interceptor';

// components
import { AppComponent } from './app.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ActivateAccountComponent } from './components/activate-account/activate-account.component';
import { FooterComponent } from './components/footer/footer.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/logIn/logIn.component';
import { ProfileComponent } from './components/profile/profile.component';
import { UpdateProfileComponent } from './components/profile-update-page/update-profile.component';
import { ProfileDeletedPageComponent } from './components/profile-deleted-page/profile-deleted-page.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ToastMessageComponent } from './components/helper-components/toast-message/toast-message.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PurchaseHistoryComponent } from './modals/purchase-history/purchase-history.component';
import { RequestComponent } from './modals/request/request.component';
import { BaseModalComponent } from './modals/base-modal/base-modal.component';
import { HttpInterceptorInterceptor } from './http-interceptors/http-interceptor.interceptor';
import { AuthInterceptorInterceptor } from './http-interceptors/auth/auth-interceptor.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    ActivateAccountComponent,
    FooterComponent,
    NavigationComponent,
    HomeComponent,
    LogInComponent,
    ProfileComponent,
    UpdateProfileComponent,
    ProfileDeletedPageComponent,
    SpinnerComponent,
    ResetPasswordComponent,
    ToastMessageComponent,
    PurchaseHistoryComponent,
    RequestComponent,
    BaseModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
