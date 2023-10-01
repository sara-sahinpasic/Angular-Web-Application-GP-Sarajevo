import { TicketModel } from "../Admin/Ticket/TicketModel";
import { PaymentMethodModel } from "../payment-method/PaymentMethodModel";
import { SelectedRoute } from "../routes/SelectedRoute";

export interface CheckoutModel {
  cardType?: TicketModel,
  quantity: number,
  paymentMethod?: PaymentMethodModel,
  selectedRoute: SelectedRoute,
}
