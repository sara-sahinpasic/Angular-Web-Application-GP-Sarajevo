import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivateAccountComponent } from './components/activate-account/activate-account.component';
import { RegistrationComponent } from './components/registration/registration.component';

const routes: Routes = [
  { path: "registracija", component: RegistrationComponent },
  { path: "activate/:token", component: ActivateAccountComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
