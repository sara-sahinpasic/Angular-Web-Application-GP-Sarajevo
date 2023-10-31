import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { VehicleTypeDto } from 'src/app/models/Admin/Vehicle/VehicleTypeDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { AdminVehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-vehicle-type-modal',
  templateUrl: './admin-vehicle-type-modal.component.html',
  styleUrls: ['./admin-vehicle-type-modal.component.scss'],
})
export class AdminVehicleTypeModalComponent implements OnInit {

  protected deleteIcon = faTrash;
  protected registrationForm!: FormGroup;
  protected vehicleTypes: Array<VehicleTypeDto> = [];
  protected vehicleTypeModel: VehicleTypeDto = {
    name: '',
  };

  constructor(
    protected formBuilder: FormBuilder,
    protected vehicleService: AdminVehicleService,
    protected modalService: ModalService
  ) {}

  ngOnInit() {
    this.inizializeValidators();
    this.getAllVehicleTypes();
  }

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
        .subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  deleteVehicleType(vehicleType: VehicleTypeDto) {
    this.vehicleService.hasVehicleAnyVehicleTypeDependencies(vehicleType.id!)
      .subscribe((hasDependenciesResponse: boolean) => this.promptVehicleTypeDeletion(hasDependenciesResponse, vehicleType));
  }

  private promptVehicleTypeDeletion(hasDependenciesResponse: boolean, vehicleType: VehicleTypeDto) {
    if (!hasDependenciesResponse) {
      this.vehicleService.deleteVehicleType(vehicleType.id!)
        .subscribe(this.reloadPage);

        return;
    }

    const shouldForceDelete: boolean = this.showConfirmationDialog(vehicleType);

    if (shouldForceDelete) {
      this.vehicleService.deleteVehicleType(vehicleType.id!)
        .subscribe(this.reloadPage);
    }
  }

  private showConfirmationDialog(vehiceType: VehicleTypeDto): boolean {
    return confirm(`Postoje vozila tipa ${vehiceType.name}. Ako obrišete tip vozila, obrisati ćete i sva vozila tipa ${vehiceType.name}. Da li želite nastaviti?`);
  }

  private reloadPage() {
    setTimeout(function () {
      window.location.reload();
    }, 1500);
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
