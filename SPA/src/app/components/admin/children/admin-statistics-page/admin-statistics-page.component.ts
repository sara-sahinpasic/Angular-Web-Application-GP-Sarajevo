import { Component, ViewChild } from '@angular/core';
import { Row, ChartType, GoogleChartComponent } from 'angular-google-charts';
import { tap } from 'rxjs';
import { TicketReportModel } from 'src/app/models/report/TicketReportModel';
import { TicketReportRowModel } from 'src/app/models/report/TicketReportRowModel';
import { PeriodModel } from 'src/app/models/report/periodModel';
import { RouteReportModel } from 'src/app/models/report/routeReportModel';
import { ReportService } from 'src/app/services/report/report.service';

@Component({
  selector: 'app-admin-statistics-page',
  templateUrl: './admin-statistics-page.component.html',
  styleUrls: ['./admin-statistics-page.component.scss']
})
export class AdminStatisticsPageComponent {
  @ViewChild(GoogleChartComponent)
  public readonly chart: GoogleChartComponent = {} as GoogleChartComponent;
  protected routeReport: RouteReportModel = {} as RouteReportModel;
  protected totalSum: number = 0;
  protected chartData: Row[] = [];
  protected readonly columns: any = ["Tip karte", "Prodana količina", { role: "style" }];
  protected readonly chartType: ChartType = ChartType.ColumnChart;
  protected ticketTypes: string[] = [];
  protected readonly months: number[] = Array(12).fill(1).map((value, index) => value + index);
  protected years: number[] = [];
  protected period: PeriodModel = {
    month: new Date().getMonth() + 1,
    year: new Date().getFullYear()
  };
  protected readonly chartOptions: any = {
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

  constructor(private reportService: ReportService) {
    this.years = Array(this.period.year - 1999).fill(2000).map((value, index) => value + index)
  }

  ngOnInit() {
    this.fetchStatistics();
  }

  protected fetchStatistics() {
    this.reportService.getStatisticsForPeriod(this.period.month, this.period.year)
    .pipe(
      tap(this.fillDailyChartData.bind(this))
    )
    .subscribe();

    this.reportService.getRouteReportDataForPeriod(this.period.month, this.period.year)
    .pipe(
      tap((response: RouteReportModel) => this.routeReport = response)
    )
    .subscribe();
  }

  private fillDailyChartData(reportModel: TicketReportModel) {
    const chartRows: Row[] = [];

    reportModel.data.forEach((reportRow: TicketReportRowModel, i: number) => {
      const color: string = i < this.chartColors.length ? this.chartColors[i] : "";
      chartRows.push([reportRow.cardType, reportRow.quantitySold, color]);
    });

    this.chartData = chartRows;
    this.totalSum = reportModel.totalSum;
  }
}
