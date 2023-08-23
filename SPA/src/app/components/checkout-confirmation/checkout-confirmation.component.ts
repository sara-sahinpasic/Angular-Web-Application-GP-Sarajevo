import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { CheckoutModel } from 'src/app/models/checkout/CheckoutModel';
import { CheckoutService } from 'src/app/services/checkout/checkout.service';
import { ToastMessageService } from 'src/app/services/toast/toast-message.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-checkout-confirmation',
  templateUrl: './checkout-confirmation.component.html',
  styleUrls: ['./checkout-confirmation.component.scss']
})
export class CheckoutConfirmationComponent implements OnInit {

  protected checkoutModel: CheckoutModel | null = this.checkoutService.getCheckoutModel();
  protected user?: UserProfileModel;

  constructor(
    private checkoutService: CheckoutService,
    private userService: UserService,
    private toastMessageService: ToastMessageService,
    private router: Router) {}

  ngOnInit(): void {
    if (!this.checkoutService.isCheckoutModelValid()) {
      this.router.navigateByUrl("/checkout");
      this.toastMessageService.pushErrorMessage("Invalid data was supplied. Please, check your purchase again.");
    }

    this.userService.user$.pipe(
      tap((user?: UserProfileModel) => this.user = user)
    )
    .subscribe();
  }

  protected confirmPurchase() {
    this.checkoutService.checkout()
      .subscribe(() => this.router.navigateByUrl(""));
  }
}
