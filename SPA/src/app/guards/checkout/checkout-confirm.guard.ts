import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { CheckoutService } from 'src/app/services/checkout/checkout.service';

@Injectable({
  providedIn: 'root'
})
export class CheckoutConfirmGuard implements CanActivate {

  constructor(private checkoutService: CheckoutService) {}

  canActivate(): boolean {
    return this.checkoutService.isCheckoutModelValid();
  }

}
