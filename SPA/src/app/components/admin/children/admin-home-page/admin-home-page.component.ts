import { Component, OnInit } from '@angular/core';
import { ChartType, Row } from 'angular-google-charts';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { TicketReportModel } from 'src/app/models/report/TicketReportModel';
import { TicketReportRowModel } from 'src/app/models/report/TicketReportRowModel';
import { ReportService } from 'src/app/services/report/report.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-admin-home-page',
  templateUrl: './admin-home-page.component.html',
  styleUrls: ['./admin-home-page.component.scss'],
})
export class AdminHomePageComponent implements OnInit {
  protected user!: UserProfileModel;
  protected totalSum: number = 0;
  protected dailyChartData: Row[] = [];
  protected columns: any = ["Tip karte", "Prodana količina", { role: "style" }];
  protected chartType: ChartType = ChartType.ColumnChart;
  protected ticketTypes: string[] = [];
  protected chartOptions: any = {
    backgroundColor: "transparent",
    role: "style",
    legend: "none",
    hAxis: {
      title: this.columns[0],
      textStyle: {
        bold: true
      }
    },
    vAxis: {
      title: this.columns[1],
      textStyle: {
        bold: true
      }
    },
    animation: {
      duration: 500,
      easing: "inAndOut",
      startup: true
    }
  };
  private chartColors: string[] = ["blue", "gold", "purple", "red"];

  constructor(private userService: UserService, private reportService: ReportService) {}

  ngOnInit() {
    this.userService.user$.pipe(
      tap((user?: UserProfileModel) => this.user = user as UserProfileModel)
    )
    .subscribe();

    this.reportService.getDashboardStatistics()
    .pipe(
      tap(this.fillDailyChartData.bind(this))
    )
    .subscribe();
  }

  private fillDailyChartData(reportModel: TicketReportModel) {
    reportModel.data.forEach((reportRow: TicketReportRowModel, i: number) => {
      const color: string = i < this.chartColors.length ? this.chartColors[i] : "";
      this.dailyChartData.push([reportRow.cardType, reportRow.quantitySold, color]);
    });

    this.totalSum = reportModel.totalSum;
  }
}
