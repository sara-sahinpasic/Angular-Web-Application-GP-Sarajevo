import { TicketReportRowModel } from "./TicketReportRowModel";

export interface TicketReportModel {
  data: TicketReportRowModel[],
  totalSum: number
}
