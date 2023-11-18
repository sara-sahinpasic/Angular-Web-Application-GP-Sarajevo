import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { tap } from "rxjs";
import { CheckoutModel } from "src/app/models/checkout/checkoutModel";
import { UserProfileModel } from "src/app/models/user/userProfileModel";
import { CheckoutService } from "src/app/services/checkout/checkout.service";
import { LocalizationService } from "src/app/services/localization/localization.service";
import { ToastMessageService } from "src/app/services/toast/toast-message.service";
import { UserService } from "src/app/services/user/user.service";


@Component({
  selector: 'app-checkout-confirmation',
  templateUrl: './checkout-confirmation.component.html',
  styleUrls: ['./checkout-confirmation.component.scss']
})
export class CheckoutConfirmationComponent implements OnInit {
  protected userDiscount: number = 0;
  protected checkoutModel: CheckoutModel = this.checkoutService.getCheckoutModel()!;
  protected user: UserProfileModel = {} as UserProfileModel;

  constructor(
    private checkoutService: CheckoutService,
    private userService: UserService,
    private toastMessageService: ToastMessageService,
    private router: Router,
    protected localizationService: LocalizationService) {}

  ngOnInit() {
    if (!this.checkoutService.isCheckoutModelValid()) {
      this.toastMessageService.pushErrorMessage('Invalid data was supplied. Please, check your purchase again.');
      this.router.navigateByUrl('/checkout');
    }

    this.userService.user$.pipe(
      tap((user?: UserProfileModel) => this.user = user!)
    )
    .subscribe();

    this.getDiscount();
  }

  protected confirmPurchase() {
    this.checkoutService.checkout()
      .subscribe(() => this.router.navigateByUrl(''));
  }

  private getDiscount() {
    this.userService.getUserDiscount()
      .pipe(
        tap((response: number) => this.userDiscount = response)
      )
      .subscribe();
  }
}
