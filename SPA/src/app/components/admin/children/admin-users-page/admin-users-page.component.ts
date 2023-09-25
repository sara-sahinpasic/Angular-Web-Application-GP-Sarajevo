import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserGetDto } from 'src/app/models/Admin/User/UserGetDto';
import { UserUpdateDto } from 'src/app/models/Admin/User/UserUpdateDto';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { AdminUserCreateService } from 'src/app/services/admin/user/admin-user-create.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-admin-users-page',
  templateUrl: './admin-users-page.component.html',
  styleUrls: ['./admin-users-page.component.scss'],
})
export class AdminUsersPageComponent implements OnInit {
  constructor(
    protected _adminUserService: AdminUserCreateService,
    protected _userService: UserService,
    protected _modalService: ModalService
  ) {}

  ngOnInit(): void {
    this._adminUserService.getAllUsers().subscribe((x: any) => {
      this.usersData = x.data;
      this.usersDataTemp = this.usersData;
    });
  }

  usersData: Array<UserGetDto> = [];
  usersDataTemp: Array<UserGetDto> = [];
  updateUserModel: UserUpdateDto = {};
  protected userModel: UserGetDto = {};

  resetData() {
    this.usersData = this.usersDataTemp;
  }

  findEmail(event: KeyboardEvent) {
    const eventTarget: HTMLInputElement = event.target as HTMLInputElement;
    const value: string = eventTarget.value;

    this.onPageChange(event);
    this.resetData();
    this.usersData = this.usersData.filter(
      (e: any) => e.email.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
  }

  showUpdateUserModal(user: UserGetDto) {
    this._modalService.data = user.id;
    this._modalService.adminShowUpdateUserModal();
  }

  deleteUser(id: any) {
    const answer: boolean = confirm('Da li želite obrisati račun?');

    if (answer == true) {
      this._userService.deleteUser(id, null).subscribe(() => {
        setTimeout(function () {
          window.location.reload();
        }, 3000);
      });
    }
  }

  //Pagination
  public paginationModel: Pagination = {
    pageSize: 5,
    page: 1,
    totalCount: 0,
  };
  onPageChange(event) {
    this.paginationModel.page = event;
  }
}
