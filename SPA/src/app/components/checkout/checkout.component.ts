import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { TicketModel } from 'src/app/models/card-type/CardTypeModel';
import { CheckoutModel } from 'src/app/models/checkout/CheckoutModel';
import { PaymentMethodModel } from 'src/app/models/payment-method/PaymentMethodModel';
import { CheckoutService } from 'src/app/services/checkout/checkout.service';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { PaymentMethodService } from 'src/app/services/payment/payment-method.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {

  protected checkoutModel: CheckoutModel = {
    quantity: 1
  };
  protected formGroup!: FormGroup;
  protected cardTypes: TicketModel[] = [];
  protected paymentMethods: PaymentMethodModel[] = [];

  constructor(
    private checkoutService: CheckoutService,
    private paymentMethodService: PaymentMethodService,
    private ticketService: TicketService,
    private formBuilder: FormBuilder,
    private router: Router,
    protected localizationService: LocalizationService) {

      if (this.checkoutService.isCheckoutModelValid()) {
        this.checkoutModel = this.checkoutService.getCheckoutModel() as CheckoutModel;
      }
    }

  ngOnInit(): void {
    this.initializeFormValidators();

    this.ticketService.getAllTickets().pipe(
      tap((response: DataResponse<TicketModel[]>) => this.cardTypes = response.data)
    )
    .subscribe();

    this.paymentMethodService.getPaymentMethods().pipe(
      tap((response: DataResponse<PaymentMethodModel[]>) => this.paymentMethods = response.data)
    )
    .subscribe();
  }

  protected checkout() {
    this.formGroup.markAllAsTouched();

    if (!this.formGroup?.valid) {
      return;
    }

    this.populateModel();
    this.checkoutService.setCheckoutModel(this.checkoutModel);
    this.router.navigateByUrl("/checkout/confirmation");
  }

  private validateQuantity(control: FormControl) {
    if (!control.value || control.value < 0) {
      return {
        quantity: false
      }
    }

    return null;
  }

  private initializeFormValidators() {
    this.formGroup = this.formBuilder.group({
      quantity: [this.checkoutModel.quantity, this.validateQuantity],
      cardTypeId: [this.checkoutModel.cardType?.id, Validators.required],
      paymentMethodId: [this.checkoutModel.paymentMethod?.id, Validators.required]
    });
  }

  private populateModel() {
    this.checkoutModel.quantity = this.formGroup.get("quantity")?.value;
    this.checkoutModel.cardType = this.cardTypes.find((cardType: TicketModel) => cardType.id == this.formGroup.get("cardTypeId")?.value);
    this.checkoutModel.paymentMethod = this.paymentMethods
                                        .find((paymentMethod: PaymentMethodModel) => paymentMethod.id == this.formGroup.get("paymentMethodId")?.value);
  }
}
