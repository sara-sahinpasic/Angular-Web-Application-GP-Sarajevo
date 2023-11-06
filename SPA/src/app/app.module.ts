import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoadingInterceptor } from './http-interceptors/loading.interceptor';
import { NgxPaginationModule } from 'ngx-pagination';

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
import { CheckoutComponent } from './components/checkout/checkout.component';
import { CheckoutConfirmationComponent } from './components/checkout-confirmation/checkout-confirmation.component';
import { ReviewComponent } from './components/review/review.component';
import { ReviewModalComponent } from './modals/review-modal/review-modal.component';
import { NewsComponent } from './components/news/news.component';
import { NewsModalComponent } from './modals/news-modal/news-modal.component';
import { RoutesComponent } from './components/routes/routes.component';
import { AdminHomePageComponent } from './components/admin/children/admin-home-page/admin-home-page.component';
import { AdminUsersPageComponent } from './components/admin/children/admin-users-page/admin-users-page.component';
import { AdminNavPageComponent } from './components/admin/admin-nav-page/admin-nav-page.component';
import { AdminNewsModalComponent } from './modals/admin/admin-news-modal/admin-news-modal.component';
import { AdminTicketModalComponent } from './modals/admin/admin-ticket-modal/admin-ticket-modal.component';
import { AdminVehicleModalComponent } from './modals/admin/admin-vehicle-modal/admin-vehicle-modal.component';
import { AdminStationModalComponent } from './modals/admin/admin-station-modal/admin-station-modal.component';
import { AdminUserModalComponent } from './modals/admin/admin-user-modal/admin-user-modal.component';
import { AdminNewStationModalComponent } from './modals/admin/admin-new-station-modal/admin-new-station-modal.component';
import { AdminCompanyPageComponent } from './components/admin/children/admin-company-page/admin-company-page.component';
import { AdminUpdateUserModalComponent } from './modals/admin/admin-update-user-modal/admin-update-user-modal.component';
import { AdminComponent } from './components/admin/admin.component';
import { AdminManufacturerModalComponent } from './modals/admin/admin-manufacturer-modal/admin-manufacturer-modal.component';
import { AdminVehicleTypeModalComponent } from './modals/admin/admin-vehicle-type-modal/admin-vehicle-type-modal.component';
import { DriverDelayPageComponent } from './components/driver/children/driver-delay-page/driver-delay-page.component';
import { GoogleChartsModule } from 'angular-google-charts';
import { AdminTicketsPageComponent } from './components/admin/children/admin-tickets-page/admin-tickets-page.component';
import { AdminNewsPageComponent } from './components/admin/children/admin-news-page/admin-news-page.component';
import { AdminVehiclePageComponent } from './components/admin/children/admin-vehicle-page/admin-vehicle-page.component';
import { DriverMalfunctionPageComponent } from './components/driver/children/driver-malfunction-page/driver-malfunction-page.component';
import { DriverNavPageComponent } from './components/driver/driver-nav-page/driver-nav-page.component';
import { DriverComponent } from './components/driver/driver.component';
import { AdminRoutesPageComponent } from './components/admin/children/admin-routes-page/admin-routes-page.component';
import { AdminRouteModalComponent } from './modals/admin/admin-route-modal/admin-route-modal.component';
import { AdminRouteEditModalComponent } from './modals/admin/admin-route-edit-modal/admin-route-edit-modal.component';
import { RoutesFormComponent } from './components/template/routes-form/routes-form.component';

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
    AdminNewStationModalComponent,
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
    RoutesFormComponent
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
