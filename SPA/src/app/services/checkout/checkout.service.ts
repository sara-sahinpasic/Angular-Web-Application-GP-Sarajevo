import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { CheckoutModel } from 'src/app/models/checkout/CheckoutModel';
import { environment } from 'src/environments/environment';
import { FinishCheckoutModel } from 'src/app/models/checkout/FinishCheckoutModel';
import { UserService } from '../user/user.service';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  private relationId?: string;
  private url: string = environment.apiUrl;
  private checkoutFinishModel: FinishCheckoutModel = {
    userId: "",
    paymentMethodId: "",
    quantity: 0,
    ticketId: ""
  };

  constructor(private httpClient: HttpClient, private userService: UserService) {
    this.userService.user$.pipe(
      tap((user?: UserProfileModel) => this.checkoutFinishModel.userId = user?.id)
    )
    .subscribe();
  }

  public setCheckoutModel(model: CheckoutModel) {
    localStorage.setItem("checkoutData", JSON.stringify(model));
  }

  public setRelation(relationId: string) {
    this.relationId = relationId;
  }

  public getCheckoutModel(): CheckoutModel | null {
    const model: string | null = localStorage.getItem("checkoutData");

    if (!model) {
      return null;
    }

    return JSON.parse(model) as CheckoutModel;
  }

  private clearCheckoutModel() {
    localStorage.removeItem("checkoutData");

    this.checkoutFinishModel = {
      userId: "",
      paymentMethodId: "",
      quantity: 0,
      ticketId: ""
    };
  }

  public checkout(): Observable<DataResponse<string>> {
    const model: CheckoutModel = this.getCheckoutModel() as CheckoutModel;

    this.checkoutFinishModel.paymentMethodId = model.paymentMethod!.id;
    this.checkoutFinishModel.quantity = model.quantity;
    this.checkoutFinishModel.ticketId = model.cardType!.id;

    return this.httpClient.post<DataResponse<string>>(`${this.url}Checkout/Finish`, this.checkoutFinishModel)
      .pipe(
        tap(this.clearCheckoutModel.bind(this))
      );
  }

  public isCheckoutModelValid(): boolean {
    const checkoutModel: CheckoutModel | null = this.getCheckoutModel();

    return !!checkoutModel && !!checkoutModel.quantity && !!this.checkoutFinishModel.userId && !!checkoutModel.cardType && !!checkoutModel.paymentMethod;
  }
}
