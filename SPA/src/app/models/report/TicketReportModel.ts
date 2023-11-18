import { TicketReportRowModel } from "./ticketReportRowModel";

export interface TicketReportModel {
  data: TicketReportRowModel[],
  totalSum: number
}
