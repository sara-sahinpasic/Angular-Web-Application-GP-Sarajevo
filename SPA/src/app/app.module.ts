import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { FormsModule } from '@angular/forms';
import { ActivateAccountComponent } from './components/activate-account/activate-account.component';
import { FooterComponent } from './components/footer/footer.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/logIn/logIn.component';

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    ActivateAccountComponent,
    FooterComponent,
    NavigationComponent,
    HomeComponent,
    LogInComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
