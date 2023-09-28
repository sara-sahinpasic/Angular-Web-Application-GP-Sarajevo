import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { TicketReportModel } from 'src/app/models/report/TicketReportModel';
import { environment } from 'src/environments/environment';
import { FileService } from '../file/file.service';

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

  public downloadPurchaseHistoryReport() {
    this.fileService.download('Report/DownloadPurchaseHistoryReport').subscribe();
  }
}
