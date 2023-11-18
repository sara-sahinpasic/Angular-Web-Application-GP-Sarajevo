import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { UserCreateModel } from 'src/app/models/admin/user/userCreateModel';
import { UserRoleDto } from 'src/app/models/admin/user/userRoleDto';
import { DataResponse } from 'src/app/models/dataResponse';
import { AdminUserService } from 'src/app/services/admin/user/admin-user-service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-user-modal',
  templateUrl: './admin-user-modal.component.html',
  styleUrls: ['./admin-user-modal.component.scss'],
})
export class AdminUserModalComponent implements OnInit {
  protected roles: UserRoleDto[] = [];
  protected userCreateModel: UserCreateModel = {} as UserCreateModel;
  protected userCreationForm: FormGroup = {} as FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private adminUserService: AdminUserService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.getAllRoles();
  }

  private getAllRoles() {
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

  protected addUser() {
    this.userCreationForm.markAllAsTouched();
    if (this.userCreationForm.valid) {
      this.adminUserService.createUser(this.userCreateModel).subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  private initializeForm() {
    this.userCreationForm = this.formBuilder.group(
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
