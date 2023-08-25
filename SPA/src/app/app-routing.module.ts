import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivateAccountComponent } from './components/activate-account/activate-account.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/logIn/logIn.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ProfileComponent } from './components/profile/profile.component';
import { UpdateProfileComponent } from './components/profile-update-page/update-profile.component';
import { ProfileDeletedPageComponent } from './components/profile-deleted-page/profile-deleted-page.component';
import { LoggedInGuard } from './guards/logged-in.guard';
import { NotLoggedInGuard } from './guards/not-logged-in.guard';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { PurchaseHistoryComponent } from './modals/purchase-history/purchase-history.component';
import { RequestComponent } from './modals/request/request.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { CheckoutConfirmationComponent } from './components/checkout-confirmation/checkout-confirmation.component';
import { CheckoutConfirmGuard } from './guards/checkout/checkout-confirm.guard';
import { ReviewComponent } from './components/review/review.component';
import { NewsComponent } from './components/news/news.component';

const routes: Routes = [
  { path: 'checkout/confirmation', component: CheckoutConfirmationComponent, canActivate: [LoggedInGuard, CheckoutConfirmGuard] },
  { path: 'checkout', component: CheckoutComponent, canActivate: [LoggedInGuard] },
  { path: 'news', component: NewsComponent},
  { path: 'review', component: ReviewComponent},
  { path: 'request', component: RequestComponent, canActivate:[LoggedInGuard] },
  { path: 'purchaseHistory', component: PurchaseHistoryComponent, canActivate:[LoggedInGuard] },
  { path: 'delete', component: ProfileDeletedPageComponent, canActivate:[LoggedInGuard] },
  { path: 'update', component: UpdateProfileComponent, canActivate:[LoggedInGuard] },
  { path: 'profile', component: ProfileComponent, canActivate:[LoggedInGuard] },
  { path: 'registracija', component: RegistrationComponent, canActivate:[NotLoggedInGuard]},
  { path: 'activate/:token', component: ActivateAccountComponent,canActivate:[NotLoggedInGuard] },
  { path: 'prijava', component: LogInComponent, canActivate:[NotLoggedInGuard] },
  { path: 'resetPassword', component: ResetPasswordComponent, canActivate: [NotLoggedInGuard]  },
  { path: '', component: HomeComponent },
  { path: '**', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
