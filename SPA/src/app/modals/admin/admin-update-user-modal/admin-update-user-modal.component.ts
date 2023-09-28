import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UserUpdateDto } from 'src/app/models/Admin/User/UserUpdateDto';
import { UserService } from 'src/app/services/user/user.service';
import { AdminUserService } from 'src/app/services/admin/user/admin-user-service';

@Component({
  selector: 'app-admin-update-user-modal',
  templateUrl: './admin-update-user-modal.component.html',
  styleUrls: ['./admin-update-user-modal.component.scss'],
})
export class AdminUpdateUserModalComponent implements OnInit {
  constructor(
    protected userService: UserService,
    protected adminService: AdminUserService
  ) {}

  @Input() userId?: string;
  updateUserModel: UserUpdateDto = {};
  registrationForm!: FormGroup;

  ngOnInit(): void {
    this.adminService.getUserById(this.userId!).subscribe((x: any) => {
      this.updateUserModel = x.data;
    });
  }

  save() {
    this.userService.updateUser(this.updateUserModel).subscribe(() => {
      setTimeout(function () {
        window.location.reload();
      }, 3000);
    });
  }
}
