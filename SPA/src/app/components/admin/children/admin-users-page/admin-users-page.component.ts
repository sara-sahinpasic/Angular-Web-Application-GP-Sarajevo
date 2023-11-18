import { Component, OnInit } from '@angular/core';
import {
  faEdit,
  faFile,
  faFilePdf,
  faTrash,
} from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { UserGetDto } from 'src/app/models/admin/user/userGetDto';
import { Pagination } from 'src/app/models/pagination/pagination';
import { UserProfileModel } from 'src/app/models/user/userProfileModel';
import { AdminUserService } from 'src/app/services/admin/user/admin-user-service';
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
  protected usersData: UserGetDto[] = [];
  protected usersDataTemp: UserGetDto[] = [];

  constructor(
    private adminUserService: AdminUserService,
    private userService: UserService,
    private reportService: ReportService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.adminUserService.getAllUsers().subscribe((x: any) => {
      this.usersData = x.data;
      this.usersDataTemp = this.usersData;
    });

    this.userService.user$
      .pipe(tap((user?: UserProfileModel) => (this.userId = user!.id!)))
      .subscribe();
  }

  private resetData() {
    this.usersData = this.usersDataTemp;
  }

  protected findByEmail(event: KeyboardEvent) {
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
    this.modalService.data = user.id;
    this.modalService.adminShowUpdateUserModal();
  }

  protected deleteUser(id: any) {
    const answer: boolean = confirm('Da li želite obrisati račun?');

    if (answer == true) {
      this.userService.deleteUser(id, null).subscribe(this.reloadPage);
    }
  }

  protected showRequestModal(userId: string) {
    this.modalService.data = userId;
    this.modalService.adminShowUserRequestModal();
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
