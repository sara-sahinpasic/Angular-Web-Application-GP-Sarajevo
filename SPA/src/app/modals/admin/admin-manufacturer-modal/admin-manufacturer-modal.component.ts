import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { ManufacturerDto } from 'src/app/models/Admin/Vehicle/ManufacturerDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { AdminVehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-manufacturer-modal',
  templateUrl: './admin-manufacturer-modal.component.html',
  styleUrls: ['./admin-manufacturer-modal.component.scss'],
})
export class AdminManufacturerModalComponent implements OnInit {
  constructor(
    protected formBuilder: FormBuilder,
    protected vehicleService: AdminVehicleService,
    protected modalService: ModalService
  ) {}
  ngOnInit(): void {
    this.inizializeValidators();
    this.getManufacturers();
  }
  protected registrationForm!: FormGroup;

  protected manufactures: Array<ManufacturerDto> = [];
  protected manufacturerModel: ManufacturerDto = {
    id: '',
    name: '',
  };

  getManufacturers() {
    this.vehicleService
      .getAllManufacturers()
      .pipe(
        tap((response: DataResponse<ManufacturerDto[]>) => {
          this.manufactures = response.data;
        })
      )
      .subscribe();
  }

  addNewManufacturer() {
    this.registrationForm.markAllAsTouched();
    if (this.registrationForm.valid) {
      this.vehicleService
        .addNewManufacturer(this.manufacturerModel)
        .subscribe(this.modalService.closeModal.bind(this.modalService));
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
