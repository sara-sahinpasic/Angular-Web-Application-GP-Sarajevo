import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivateAccountComponent } from './components/activate-account/activate-account.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/logIn/logIn.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ProfileComponent } from './components/profile/profile.component';
import { EditProfilePage } from './components/profile/edit-page/edit-page.component';
import { ProfileDeletedPageComponent } from './components/profile/deleted-page/deleted-page.component';
import { LoggedInGuard } from './guards/logged-in.guard';
import { NotLoggedInGuard } from './guards/not-logged-in.guard';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { CheckoutConfirmationComponent } from './components/checkout-confirmation/checkout-confirmation.component';
import { CheckoutConfirmGuard } from './guards/checkout/checkout-confirm.guard';
import { ReviewComponent } from './components/review/review.component';
import { NewsComponent } from './components/news/news.component';
import { RoutesComponent } from './components/routes/routes.component';
import { RouteNotSetGuard } from './guards/routes/route-not-set.guard';
import { AdminHomePageComponent } from './components/admin/children/admin-home-page/admin-home-page.component';
import { AdminUsersPageComponent } from './components/admin/children/admin-users-page/admin-users-page.component';
import { AdminCompanyPageComponent } from './components/admin/children/admin-company-page/admin-company-page.component';
import { AdminGuard } from './guards/admin/admin.guard';
import { AdminComponent } from './components/admin/admin.component';
import { AdminTicketsPageComponent } from './components/admin/children/admin-tickets-page/admin-tickets-page.component';
import { AdminNewsPageComponent } from './components/admin/children/admin-news-page/admin-news-page.component';
import { AdminVehiclePageComponent } from './components/admin/children/admin-vehicle-page/admin-vehicle-page.component';
import { DriverComponent } from './components/driver/driver.component';
import { DriverDelayPageComponent } from './components/driver/children/driver-delay-page/driver-delay-page.component';
import { DriverMalfunctionPageComponent } from './components/driver/children/driver-malfunction-page/driver-malfunction-page.component';
import { AdminRoutesPageComponent } from './components/admin/children/admin-routes-page/admin-routes-page.component';
import { AdminHolidaysPageComponent } from './components/admin/children/admin-holidays-page/admin-holidays-page.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { AdminStatisticsPageComponent } from './components/admin/children/admin-statistics-page/admin-statistics-page.component';
import { PurchaseHistoryComponent } from './components/purchase-history/purchase-history.component';
import { DriverGuard } from './guards/admin/driver.guard';

const routes: Routes = [
  { path: 'routes', component: RoutesComponent, canActivate: [RouteNotSetGuard] },
  { path: 'driver', component: DriverComponent, canActivate: [DriverGuard, LoggedInGuard], children: [
    { path: 'delay', component: DriverDelayPageComponent, outlet: 'driver'},
    { path: 'malfunction', component: DriverMalfunctionPageComponent, outlet: 'driver'},
  ]},
  { path: 'admin', component: AdminComponent, canActivate: [LoggedInGuard, AdminGuard], children: [
    { path: 'home', component: AdminHomePageComponent, outlet: 'admin'},
    { path: 'users', component: AdminUsersPageComponent, outlet: 'admin'},
    { path: 'tickets', component: AdminTicketsPageComponent, outlet: 'admin'},
    { path: 'company', component: AdminCompanyPageComponent, outlet: 'admin' },
    { path: 'news', component: AdminNewsPageComponent, outlet: 'admin' },
    { path: 'vehicles', component: AdminVehiclePageComponent, outlet: 'admin' },
    { path: 'routes', component: AdminRoutesPageComponent, outlet: 'admin' },
    { path: 'holidays', component: AdminHolidaysPageComponent, outlet: 'admin' },
    { path: 'report', component: AdminStatisticsPageComponent, outlet: 'admin' }
  ]},
  { path: 'checkout/confirmation', component: CheckoutConfirmationComponent, canActivate: [RouteNotSetGuard, LoggedInGuard, CheckoutConfirmGuard] },
  { path: 'checkout', component: CheckoutComponent, canActivate: [RouteNotSetGuard] },
  { path: 'news', component: NewsComponent },
  { path: 'review', component: ReviewComponent },
  { path: 'purchaseHistory', component: PurchaseHistoryComponent, canActivate:[LoggedInGuard] },
  { path: 'delete', component: ProfileDeletedPageComponent, canActivate:[LoggedInGuard] },
  { path: 'update', component: EditProfilePage, canActivate: [LoggedInGuard] },
  { path: 'profile', component: ProfileComponent, canActivate:[LoggedInGuard] },
  { path: 'register', component: RegistrationComponent, canActivate:[NotLoggedInGuard] },
  { path: 'activate/:token', component: ActivateAccountComponent,canActivate:[NotLoggedInGuard] },
  { path: 'login', component: LogInComponent, canActivate:[NotLoggedInGuard] },
  { path: 'resetPassword', component: ResetPasswordComponent, canActivate: [NotLoggedInGuard] },
  { path: '', component: HomeComponent},
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
