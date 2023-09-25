import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { VehicleTypeDto } from 'src/app/models/Admin/Vehicle/VehicleTypeDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { AdminVehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';

@Component({
  selector: 'app-admin-vehicle-type-modal',
  templateUrl: './admin-vehicle-type-modal.component.html',
  styleUrls: ['./admin-vehicle-type-modal.component.scss'],
})
export class AdminVehicleTypeModalComponent implements OnInit {
  constructor(
    protected formBuilder: FormBuilder,
    protected vehicleService: AdminVehicleService
  ) {}

  ngOnInit(): void {
    this.inizializeValidators();
    this.getAllVehicleTypes();
  }
  protected registrationForm!: FormGroup;
  protected vehicleTypes: Array<VehicleTypeDto> = [];
  protected vehicleTypeModel: VehicleTypeDto = {
    id: '',
    name: '',
  };

  getAllVehicleTypes() {
    this.vehicleService
      .getAllVehicleTypes()
      .pipe(
        tap((response: DataResponse<VehicleTypeDto[]>) => {
          this.vehicleTypes = response.data;
        })
      )
      .subscribe();
  }

  addNewVehicleType() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.vehicleService
        .addNewVehicleType(this.vehicleTypeModel)
        .subscribe(() => {
          setTimeout(function () {
            window.location.reload();
          }, 3000);
        });
    }
  }

  inizializeValidators() {
    this.registrationForm = this.formBuilder.group({
      name: ['', Validators.required],
    });
  }

  //Pagination
  public paginationModel: Pagination = {
    pageSize: 3,
    page: 1,
    totalCount: 0,
  };
  onPageChange(event) {
    this.paginationModel.page = event;
  }
}
