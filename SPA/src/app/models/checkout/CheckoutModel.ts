import { TicketModel } from "../admin/ticket/ticketModel";
import { PaymentMethodModel } from "../payment-method/paymentMethodModel";
import { SelectedRoute } from "../routes/selectedRoute";

export interface CheckoutModel {
  cardType?: TicketModel,
  quantity: number,
  paymentMethod?: PaymentMethodModel,
  selectedRoute: SelectedRoute
}
