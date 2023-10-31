import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserCreateModel } from 'src/app/models/Admin/User/UserCreateModel';
import { UserRoleDto } from 'src/app/models/Admin/User/UserRoleDto';
import { AdminUserCreateService } from 'src/app/services/admin/user/admin-user-create.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-user-modal',
  templateUrl: './admin-user-modal.component.html',
  styleUrls: ['./admin-user-modal.component.scss'],
})
export class AdminUserModalComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private adminUserService: AdminUserCreateService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.getAllRoles();
  }
  protected roles: Array<UserRoleDto> = [];

  protected userCreateModel: UserCreateModel = {
    firstName: '',
    lastName: '',
    email: '',
    dateOfBirth: undefined,
    password: '',
    roleId: '',
  };
  registrationForm!: FormGroup;

  getAllRoles() {
    this.adminUserService
      .getAllRoles()
      .pipe(
        tap((response: DataResponse<UserRoleDto[]>) => {
          this.roles = response.data;
          this.userCreateModel.roleId = response.data[0].id;
        })
      )
      .subscribe();
  }

  addUser() {
    this.registrationForm.markAllAsTouched();
    if (this.registrationForm.valid) {
      this.adminUserService.createUser(this.userCreateModel).subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  initForm() {
    this.registrationForm = this.formBuilder.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        email: [
          '',
          Validators.pattern(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/),
        ],
        password: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }
}
