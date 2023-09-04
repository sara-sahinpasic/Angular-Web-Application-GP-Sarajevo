export interface FinishCheckoutModel {
  userId?: string,
  ticketId: string,
  paymentMethodId: string,
  quantity: number,
  routeId: string,
  date?: string
}
