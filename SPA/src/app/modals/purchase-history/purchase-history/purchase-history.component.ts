import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  Component,
  OnInit,
} from '@angular/core';
import { DataResponse } from 'src/app/models/DataResponse';
import { PurchaseHistoryDto } from 'src/app/models/Purchase-history/PurchaseHistoryDto';
import { environment } from 'src/environments/environment';
import { FileService } from 'src/app/services/file-upload/file.service';
import { tap } from 'rxjs';
import { UserService } from 'src/app/services/user/user.service';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
@Component({
  selector: 'app-purchase-history',
  templateUrl: './purchase-history.component.html',
  styleUrls: ['./purchase-history.component.scss'],
})
export class PurchaseHistoryComponent implements OnInit {
  purchaseHistory: Array<PurchaseHistoryDto> = [];

  private url: string = environment.apiUrl;

  constructor(
    private httpClient: HttpClient,
    private fileService: FileService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    let userId: string | undefined;

    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => userId = user?.id)
    )
    .subscribe();

    const token: string | null = localStorage.getItem('token');
    const headers: HttpHeaders = new HttpHeaders({
      Authorization: 'Bearer ' + token,
    });

    this.httpClient
      .get<DataResponse<PurchaseHistoryDto[]>>(`${this.url}PurchaseHistory/GetAllUserPurchases?userId=${userId}`,
      {
        headers,
      })
      .subscribe((r: DataResponse<PurchaseHistoryDto[]>) => {
        this.purchaseHistory = r.data;
      });
  }

  purchaseHistoryPrintButton() {
    this.fileService.download("File/DownloadPurchaseHistory")
      .subscribe();
  }
}
