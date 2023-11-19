import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoadingInterceptor } from './http-interceptors/loading.interceptor';
import { NgxPaginationModule } from 'ngx-pagination';

// components
import { VerifyLoginComponent } from './components/logIn/verify-login/verify-login.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { GoogleChartsModule } from 'angular-google-charts';
import { AppComponent } from './app.component';
import { ActivateAccountComponent } from './components/activate-account/activate-account.component';
import { AdminNavPageComponent } from './components/admin/admin-nav-page/admin-nav-page.component';
import { AdminComponent } from './components/admin/admin.component';
import { AdminCompanyPageComponent } from './components/admin/children/admin-company-page/admin-company-page.component';
import { AdminHolidaysPageComponent } from './components/admin/children/admin-holidays-page/admin-holidays-page.component';
import { AdminHomePageComponent } from './components/admin/children/admin-home-page/admin-home-page.component';
import { AdminNewsPageComponent } from './components/admin/children/admin-news-page/admin-news-page.component';
import { AdminRoutesPageComponent } from './components/admin/children/admin-routes-page/admin-routes-page.component';
import { AdminStatisticsPageComponent } from './components/admin/children/admin-statistics-page/admin-statistics-page.component';
import { AdminTicketsPageComponent } from './components/admin/children/admin-tickets-page/admin-tickets-page.component';
import { AdminUsersPageComponent } from './components/admin/children/admin-users-page/admin-users-page.component';
import { AdminVehiclePageComponent } from './components/admin/children/admin-vehicle-page/admin-vehicle-page.component';
import { CheckoutConfirmationComponent } from './components/checkout-confirmation/checkout-confirmation.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { DriverDelayPageComponent } from './components/driver/children/driver-delay-page/driver-delay-page.component';
import { DriverMalfunctionPageComponent } from './components/driver/children/driver-malfunction-page/driver-malfunction-page.component';
import { DriverNavPageComponent } from './components/driver/driver-nav-page/driver-nav-page.component';
import { DriverComponent } from './components/driver/driver.component';
import { FooterComponent } from './components/footer/footer.component';
import { ToastMessageComponent } from './components/helper-components/toast-message/toast-message.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/logIn/logIn.component';
import { AdminHolidayModalComponent } from './components/modals/admin/admin-holiday-modal/admin-holiday-modal.component';
import { AdminManufacturerModalComponent } from './components/modals/admin/admin-manufacturer-modal/admin-manufacturer-modal.component';
import { AdminNewsModalComponent } from './components/modals/admin/admin-news-modal/admin-news-modal.component';
import { AdminRouteEditModalComponent } from './components/modals/admin/admin-route-edit-modal/admin-route-edit-modal.component';
import { AdminRouteModalComponent } from './components/modals/admin/admin-route-modal/admin-route-modal.component';
import { AdminStationModalComponent } from './components/modals/admin/admin-station-modal/admin-station-modal.component';
import { AdminTicketModalComponent } from './components/modals/admin/admin-ticket-modal/admin-ticket-modal.component';
import { AdminUpdateUserModalComponent } from './components/modals/admin/admin-update-user-modal/admin-update-user-modal.component';
import { AdminUserModalComponent } from './components/modals/admin/admin-user-modal/admin-user-modal.component';
import { AdminUserRequestModalComponent } from './components/modals/admin/admin-user-request-modal/admin-user-request-modal.component';
import { AdminVehicleModalComponent } from './components/modals/admin/admin-vehicle-modal/admin-vehicle-modal.component';
import { AdminVehicleTypeModalComponent } from './components/modals/admin/admin-vehicle-type-modal/admin-vehicle-type-modal.component';
import { BaseModalComponent } from './components/modals/base-modal/base-modal.component';
import { NewsModalComponent } from './components/modals/news-modal/news-modal.component';
import { RequestModalComponent } from './components/modals/request-modal/request-modal.component';
import { ReviewModalComponent } from './components/modals/review-modal/review-modal.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { NewsComponent } from './components/news/news.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { ProfileDeletedPageComponent } from './components/profile/deleted-page/deleted-page.component';
import { EditProfilePage } from './components/profile/edit-page/edit-page.component';
import { ProfileComponent } from './components/profile/profile.component';
import { PurchaseHistoryComponent } from './components/purchase-history/purchase-history.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ReviewComponent } from './components/review/review.component';
import { RoutesComponent } from './components/routes/routes.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { RoutesFormComponent } from './components/template/routes-form/routes-form.component';
import { AuthInterceptorInterceptor } from './http-interceptors/auth/auth-interceptor.interceptor';
import { HttpInterceptorInterceptor } from './http-interceptors/http-interceptor.interceptor';

import { SafePipe } from './pipes/safeUrl/safe.pipe';
import { AdminVehicleMalfunctionDetailsComponent } from './components/modals/admin/admin-vehicle-malfunction-details/admin-vehicle-malfunction-details.component';

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
    EditProfilePage,
    ProfileDeletedPageComponent,
    SpinnerComponent,
    ResetPasswordComponent,
    ToastMessageComponent,
    PurchaseHistoryComponent,
    RequestModalComponent,
    BaseModalComponent,
    CheckoutComponent,
    CheckoutConfirmationComponent,
    ReviewComponent,
    ReviewModalComponent,
    NewsComponent,
    NewsModalComponent,
    RoutesComponent,
    AdminHomePageComponent,
    AdminUsersPageComponent,
    AdminNavPageComponent,
    AdminCompanyPageComponent,
    AdminNewsModalComponent,
    AdminTicketModalComponent,
    AdminVehicleModalComponent,
    AdminStationModalComponent,
    AdminUserModalComponent,
    AdminUpdateUserModalComponent,
    AdminComponent,
    AdminManufacturerModalComponent,
    AdminVehicleTypeModalComponent,
    DriverNavPageComponent,
    DriverComponent,
    DriverDelayPageComponent,
    DriverMalfunctionPageComponent,
    AdminTicketsPageComponent,
    AdminNewsPageComponent,
    AdminVehiclePageComponent,
    AdminRoutesPageComponent,
    AdminRouteModalComponent,
    AdminRouteEditModalComponent,
    RoutesFormComponent,
    AdminUserRequestModalComponent,
    SafePipe,
    AdminHolidaysPageComponent,
    AdminHolidayModalComponent,
    PageNotFoundComponent,
    AdminStatisticsPageComponent,
    VerifyLoginComponent,
    AdminVehicleMalfunctionDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    NgxPaginationModule,
    GoogleChartsModule
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
      multi: true,
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
