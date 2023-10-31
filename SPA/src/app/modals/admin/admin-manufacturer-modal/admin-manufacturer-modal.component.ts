import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
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

  protected deleteIcon = faTrash;
  protected registrationForm!: FormGroup;
  protected manufactures: Array<ManufacturerDto> = [];
  protected manufacturerModel: ManufacturerDto = {
    id: undefined,
    name: '',
  };

  constructor(
    protected formBuilder: FormBuilder,
    protected vehicleService: AdminVehicleService,
    protected modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.inizializeValidators();
    this.getManufacturers();
  }

  protected getManufacturers() {
    this.vehicleService
      .getAllManufacturers()
      .pipe(
        tap((response: DataResponse<ManufacturerDto[]>) => {
          this.manufactures = response.data;
        })
      )
      .subscribe();
  }

  protected addNewManufacturer() {
    this.registrationForm.markAllAsTouched();
    if (this.registrationForm.valid) {
      this.vehicleService
        .addNewManufacturer(this.manufacturerModel)
        .subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  protected deleteManufacturer(manufacturer: ManufacturerDto) {
    this.vehicleService.hasVehicleAnyManufacturerDependencies(manufacturer.id!)
      .subscribe((hasDependenciesResponse: boolean) => this.promptManufacturerDeletion(hasDependenciesResponse, manufacturer));
  }

  private promptManufacturerDeletion(hasDependenciesResponse: boolean, manufacturer: ManufacturerDto) {
    if (!hasDependenciesResponse) {
      this.vehicleService.deleteManufacturer(manufacturer.id!)
        .subscribe(this.reloadPage);

        return;
    }

    const shouldForceDelete: boolean = this.showConfirmationDialog(manufacturer);

    if (shouldForceDelete) {
      this.vehicleService.deleteManufacturer(manufacturer.id!)
        .subscribe(this.reloadPage);
    }
  }

  private showConfirmationDialog(manufacturer: ManufacturerDto): boolean {
    return confirm(`Postoje vozila proizvođača ${manufacturer.name}. Ako obrišete prozvođača, obrisati ćete i sva vozila čiji je proizvođač ${manufacturer.name}. Da li želite nastaviti?`);
  }

  private reloadPage() {
    setTimeout(function () {
      window.location.reload();
    }, 1500);
  }

  private inizializeValidators() {
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

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }
}
