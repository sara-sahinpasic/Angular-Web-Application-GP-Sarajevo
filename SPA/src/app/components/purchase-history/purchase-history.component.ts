import { Component, OnInit } from '@angular/core';
import { PurchaseHistoryDto } from 'src/app/models/purchase-history/purchaseHistoryDto';
import { catchError, of, tap } from 'rxjs';
import { UserService } from 'src/app/services/user/user.service';
import { Pagination } from 'src/app/models/pagination/pagination';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ReportService } from 'src/app/services/report/report.service';
import { UserProfileModel } from 'src/app/models/user/userProfileModel';

@Component({
  selector: 'app-purchase-history',
  templateUrl: './purchase-history.component.html',
  styleUrls: ['./purchase-history.component.scss'],
})
export class PurchaseHistoryComponent implements OnInit {
  protected purchaseHistory: PurchaseHistoryDto[] = [];
  protected paginationModel: Pagination = {
    pageSize: 7,
    page: 1,
    totalCount: 0,
  };
  private userId?: string;

  constructor(
    private reportService: ReportService,
    private userService: UserService,
    protected localizationService: LocalizationService
  ) {}

  ngOnInit() {
    this.fillHistoryData();
  }

  private fillHistoryData() {
    this.userService.user$
      .pipe(
        tap((user: UserProfileModel | undefined) => {
          if (!user) {
            throw new Error('User must be defined.');
          }

          this.userId = user.id;
        }),
        catchError(() => of([]))
      )
      .subscribe(this.loadPurchaseHistory.bind(this));
  }

  private loadPurchaseHistory() {
    this.reportService.getPurchaseHistory(this.userId!)
      .pipe(
        tap((response: PurchaseHistoryDto[]) => this.purchaseHistory = response)
      )
      .subscribe();
  }

  protected purchaseHistoryPrintButton() {
    this.reportService.downloadPurchaseHistoryReport(this.userId!);
  }

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }
}
