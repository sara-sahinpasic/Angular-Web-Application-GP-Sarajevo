import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivateAccountComponent } from './components/activate-account/activate-account.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/logIn/logIn.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ProfileComponent } from './components/profile/profile.component';
import { UpdateProfileComponent } from './components/profile-update-page/update-profile.component';
import { ProfileDeletedPageComponent } from './components/profile-deleted-page/profile-deleted-page.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';

const routes: Routes = [
  { path: 'delete/:id', component: ProfileDeletedPageComponent },
  { path: 'update/', component: UpdateProfileComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'registracija', component: RegistrationComponent },
  { path: 'activate/:token', component: ActivateAccountComponent },
  { path: 'prijava', component: LogInComponent },
  { path: 'resetPassword', component: ResetPasswordComponent },
  { path: '', component: HomeComponent },
  { path: '**', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
