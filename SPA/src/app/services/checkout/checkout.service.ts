import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap, Observable } from 'rxjs';
import { CheckoutModel } from 'src/app/models/checkout/checkoutModel';
import { FinishCheckoutModel } from 'src/app/models/checkout/finishCheckoutModel';
import { DataResponse } from 'src/app/models/dataResponse';
import { UserProfileModel } from 'src/app/models/user/userProfileModel';
import { environment } from 'src/environments/environment';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  private url: string = environment.apiUrl;
  private checkoutFinishModel: FinishCheckoutModel = {
    userId: '',
    paymentMethodId: '',
    quantity: 0,
    ticketId: '',
    routeId: ''
  };
  private checkoutPlaceholderModel: CheckoutModel | null = null;

  constructor(private httpClient: HttpClient, private userService: UserService) {
    this.userService.user$.pipe(
      tap((user?: UserProfileModel) => this.checkoutFinishModel.userId = user?.id)
    )
    .subscribe();
  }

  public setCheckoutModel(model: CheckoutModel) {
    this.checkoutPlaceholderModel = model;
  }

  public getCheckoutModel(): CheckoutModel | null {
    return this.checkoutPlaceholderModel;
  }

  private clearCheckoutModel() {
    this.checkoutFinishModel = {
      userId: '',
      paymentMethodId: '',
      quantity: 0,
      ticketId: '',
      routeId: ''
    };
  }

  public checkout(): Observable<DataResponse<string>> {
    const model: CheckoutModel = this.getCheckoutModel() as CheckoutModel;

    this.mapToFinishModel(model);

    return this.httpClient.post<DataResponse<string>>(`${this.url}Checkout/Finish`, this.checkoutFinishModel)
      .pipe(
        tap(this.clearCheckoutModel.bind(this))
      );
  }

  private mapToFinishModel(model: CheckoutModel) {
    this.checkoutFinishModel.paymentMethodId = model.paymentMethod!.id;
    this.checkoutFinishModel.quantity = model.quantity;
    this.checkoutFinishModel.ticketId = model.cardType!.id;
    this.checkoutFinishModel.routeId = model.selectedRoute?.id!;
    this.checkoutFinishModel.date = model.selectedRoute?.dateStamp.date;
  }

  public isCheckoutModelValid(): boolean {
    const checkoutModel: CheckoutModel | null = this.getCheckoutModel();

    return !!checkoutModel && !!checkoutModel.quantity && !!this.checkoutFinishModel.userId
      && !!checkoutModel.cardType && !!checkoutModel.paymentMethod && !!checkoutModel.selectedRoute;
  }
}
