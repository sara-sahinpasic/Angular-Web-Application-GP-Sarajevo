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


const routes: Routes = [
  { path: 'delete', component: ProfileDeletedPageComponent, canActivate:[LoggedInGuard] },
  { path: 'update', component: UpdateProfileComponent, canActivate:[LoggedInGuard] },
  { path: 'profile', component: ProfileComponent, canActivate:[LoggedInGuard] },
  { path: 'registracija', component: RegistrationComponent, canActivate:[NotLoggedInGuard]},
  { path: 'activate/:token', component: ActivateAccountComponent,canActivate:[NotLoggedInGuard] },
  { path: 'prijava', component: LogInComponent, canActivate:[NotLoggedInGuard] },
  { path: 'resetPassword', component: ResetPasswordComponent },
  { path: '', component: HomeComponent },
  { path: '**', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
