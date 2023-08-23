import { TicketModel } from "../card-type/CardTypeModel";
import { PaymentMethodModel } from "../payment-method/PaymentMethodModel";

export interface CheckoutModel {
  cardType?: TicketModel,
  quantity: number,
  paymentMethod?: PaymentMethodModel,
  relation?: null //to be implemented also see that the values aren't null
}
