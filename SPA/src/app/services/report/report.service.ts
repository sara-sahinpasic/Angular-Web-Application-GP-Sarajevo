import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/dataResponse';
import { TicketReportModel } from 'src/app/models/report/ticketReportModel';
import { environment } from 'src/environments/environment';
import { FileService } from '../file/file.service';
import { RouteReportModel } from 'src/app/models/report/routeReportModel';
import { PurchaseHistoryDto } from 'src/app/models/purchase-history/purchaseHistoryDto';
import { InvalidArgumentException } from 'src/app/exceptions/invalidArgumentException';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient, private fileService: FileService) {}

  public getDashboardStatistics(): Observable<TicketReportModel> {
    return this.httpClient.get<DataResponse<TicketReportModel>>(`${this.apiUrl}Admin/Report/Daily`)
      .pipe(
        map((response: DataResponse<TicketReportModel>) => response.data)
      );
  }

  public downloadPurchaseHistoryReport(userId: string) {
    if (!userId) {
      throw new InvalidArgumentException(['userId']);
    }

    this.fileService.download(`Report/PurchaseHistory/${userId}`)
      .subscribe();
  }

  public getStatisticsForPeriod(month: number, year: number): Observable<TicketReportModel> {
    return this.httpClient.get<DataResponse<TicketReportModel>>(`${this.apiUrl}Admin/Report/Period?month=${month}&year=${year}`)
      .pipe(
        map((response: DataResponse<TicketReportModel>) => response.data)
      );
  }

  public getRouteReportDataForPeriod(month: number, year: number): Observable<RouteReportModel> {
    return this.httpClient.get<DataResponse<RouteReportModel>>(`${this.apiUrl}Admin/Report/MostSoldRoute/Period?month=${month}&year=${year}`)
      .pipe(
        map((response: DataResponse<RouteReportModel>) => response.data)
      );
  }

  public getPurchaseHistory(userId: string): Observable<PurchaseHistoryDto[]> {
    if (!userId) {
      throw new InvalidArgumentException(['userId']);
    }

    return this.httpClient.get<DataResponse<PurchaseHistoryDto[]>>(
      `${this.apiUrl}User/PurchaseHistory/Get/${userId}`
      )
      .pipe(
        map((response: DataResponse<PurchaseHistoryDto[]>) => response.data)
      );
  }
}
