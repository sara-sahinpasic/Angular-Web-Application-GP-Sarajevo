import { Component, OnInit } from '@angular/core';
import {
  faEdit,
  faFile,
  faFilePdf,
  faTrash,
} from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { UserGetDto } from 'src/app/models/Admin/User/UserGetDto';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { AdminUserCreateService } from 'src/app/services/admin/user/admin-user-create.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { ReportService } from 'src/app/services/report/report.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-admin-users-page',
  templateUrl: './admin-users-page.component.html',
  styleUrls: ['./admin-users-page.component.scss'],
})
export class AdminUsersPageComponent implements OnInit {
  protected userId: string = '';
  protected editIcon = faEdit;
  protected deleteIcon = faTrash;
  protected requestIcon = faFile
  protected purchaseHistoryIcon = faFilePdf;
  protected usersData: Array<UserGetDto> = [];
  protected usersDataTemp: Array<UserGetDto> = [];

  constructor(
    private _adminUserService: AdminUserCreateService,
    private _userService: UserService,
    private reportService: ReportService,
    private _modalService: ModalService
  ) {}

  ngOnInit(): void {
    this._adminUserService.getAllUsers().subscribe((x: any) => {
      this.usersData = x.data;
      this.usersDataTemp = this.usersData;
    });

    this._userService.user$
      .pipe(tap((user?: UserProfileModel) => (this.userId = user!.id!)))
      .subscribe();
  }

  private resetData() {
    this.usersData = this.usersDataTemp;
  }

  protected findEmail(event: KeyboardEvent) {
    const eventTarget: HTMLInputElement = event.target as HTMLInputElement;
    const value: string = eventTarget.value;

    this.onPageChange(event);
    this.resetData();
    this.usersData = this.usersData.filter(
      (e: any) => e.email.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
  }

  protected getUserPurchaseHistory(userId: string) {
    this.reportService.downloadPurchaseHistoryReport(userId);
  }

  protected showUpdateUserModal(user: UserGetDto) {
    this._modalService.data = user.id;
    this._modalService.adminShowUpdateUserModal();
  }

  protected deleteUser(id: any) {
    const answer: boolean = confirm('Da li želite obrisati račun?');

    if (answer == true) {
      this._userService.deleteUser(id, null).subscribe(this.reloadPage);
    }
  }

  protected showRequestModal(userId: string) {
    this._modalService.data = userId;
    this._modalService.adminShowUserRequestModal();
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  //Pagination
  public paginationModel: Pagination = {
    pageSize: 10,
    page: 1,
    totalCount: 0,
  };

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }
}
