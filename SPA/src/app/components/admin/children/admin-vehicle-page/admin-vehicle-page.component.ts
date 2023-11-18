import { Component } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/invalidArgumentException';
import { VehicleListDto } from 'src/app/models/admin/vehicle/vehicleDto';
import { Pagination } from 'src/app/models/pagination/pagination';
import { AdminVehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-vehicle-page',
  templateUrl: './admin-vehicle-page.component.html',
  styleUrls: ['./admin-vehicle-page.component.scss']
})
export class AdminVehiclePageComponent {
  protected paginationModel: Pagination = {
    pageSize: 5,
    page: 1,
    totalCount: 0,
  };
  protected vehicleList: VehicleListDto[] = [];
  protected filteredVehicleList: VehicleListDto[] = [];
  protected editIcon = faEdit;
  protected deleteIcon = faTrash;

  constructor(private vehicleService: AdminVehicleService, private modalService: ModalService) {}

  ngOnInit() {
    this.vehicleService.getVehicleList()
      .pipe(
        tap(this.loadVehicles.bind(this))
      )
      .subscribe();
  }

  private loadVehicles(data: VehicleListDto[]) {
    this.vehicleList = data;
    this.filteredVehicleList = data;
  }

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }

  protected showVehicleEditModal(vehicle: VehicleListDto) {
    this.modalService.data = vehicle;
    this.modalService.adminShowVehiclesModal(vehicle.registrationNumber);
  }

  protected deleteVehicle(vehicle: VehicleListDto) {
    if (!vehicle.id) {
      throw new InvalidArgumentException(['vehicle.id'])
    }

    this.vehicleService.deleteVehicle(vehicle)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  protected findByRegistrationNumber(event: KeyboardEvent) {
    const target: HTMLInputElement = event.target as HTMLInputElement;

    if (!target.value) {
      this.resetFilter();
      return;
    }

    this.filteredVehicleList = this.vehicleList.filter(vehicle => vehicle.registrationNumber!.toLowerCase().indexOf(target.value.toLowerCase()) > -1);
  }

  private resetFilter() {
    this.filteredVehicleList = this.vehicleList;
  }
}
