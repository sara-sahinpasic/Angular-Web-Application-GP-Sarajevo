import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UserUpdateDto } from 'src/app/models/admin/user/userUpdateDto';
import { UserService } from 'src/app/services/user/user.service';
import { AdminUserService } from 'src/app/services/admin/user/admin-user-service';

@Component({
  selector: 'app-admin-update-user-modal',
  templateUrl: './admin-update-user-modal.component.html',
  styleUrls: ['./admin-update-user-modal.component.scss'],
})
export class AdminUpdateUserModalComponent implements OnInit {
  @Input() userId?: string;
  protected updateUserModel: UserUpdateDto = {};
  protected registrationForm: FormGroup = {} as FormGroup;

  constructor(
    protected userService: UserService,
    protected adminService: AdminUserService
  ) {}

  ngOnInit(): void {
    this.adminService.getUserById(this.userId!).subscribe((x: any) => {
      this.updateUserModel = x.data;
    });
  }

  protected save() {
    this.userService
      .updateUser(this.updateUserModel)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }
}
