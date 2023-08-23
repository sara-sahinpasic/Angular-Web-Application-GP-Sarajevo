import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { PaymentMethodModel } from 'src/app/models/payment-method/PaymentMethodModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentMethodService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  public getPaymentMethods(): Observable<DataResponse<PaymentMethodModel[]>> {
    return this.httpClient.get<DataResponse<PaymentMethodModel[]>>(`${this.url}PaymentMethod/All`);
  }
}
