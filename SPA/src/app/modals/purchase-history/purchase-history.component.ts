import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DataResponse } from 'src/app/models/DataResponse';
import { PurchaseHistoryDto } from 'src/app/models/Purchase-history/PurchaseHistoryDto';
import { environment } from 'src/environments/environment';
import { FileService } from 'src/app/services/file/file.service';
import { tap } from 'rxjs';
import { UserService } from 'src/app/services/user/user.service';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { Pagination } from 'src/app/models/Pagination/Pagination';
@Component({
  selector: 'app-purchase-history',
  templateUrl: './purchase-history.component.html',
  styleUrls: ['./purchase-history.component.scss'],
})
export class PurchaseHistoryComponent implements OnInit {
  purchaseHistory: Array<PurchaseHistoryDto> = [];

  private apiUrl: string = environment.apiUrl;

  constructor(
    private httpClient: HttpClient,
    private fileService: FileService,
    private userService: UserService
  ) {}

  public paginationModel: Pagination = {
    pageSize: 7,
    page: 1,
    totalCount: 0,
  };

  ngOnInit(): void {
    let userId: string | undefined;

    this.userService.user$
      .pipe(tap((user: UserProfileModel | undefined) => (userId = user?.id)))
      .subscribe();

    this.httpClient
      .get<DataResponse<PurchaseHistoryDto[]>>(
        `${this.apiUrl}PurchaseHistory/GetAllUserPurchases?userId=${userId}`
      )
      .subscribe((r: DataResponse<PurchaseHistoryDto[]>) => {
        this.purchaseHistory = r.data;
      });
  }

  purchaseHistoryPrintButton() {
    this.fileService.download('File/DownloadPurchaseHistory').subscribe();
  }
  onPageChange(event) {
    this.paginationModel.page = event;
  }
}
