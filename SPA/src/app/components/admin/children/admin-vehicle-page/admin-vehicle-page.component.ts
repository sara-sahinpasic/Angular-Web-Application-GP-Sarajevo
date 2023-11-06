import { Component } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/InvalidArgumentException';
import { VehicleListDto } from 'src/app/models/Admin/Vehicle/VehicleDto';
import { Pagination } from 'src/app/models/Pagination/Pagination';
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

  loadVehicles(data: VehicleListDto[]) {
    this.vehicleList = data;
    this.filteredVehicleList = data;
  }

  onPageChange(event) {
    this.paginationModel.page = event;
  }

  showVehicleEditModal(vehicle: VehicleListDto) {
    this.modalService.data = vehicle;
    this.modalService.adminShowVehiclesModal(vehicle.registrationNumber);
  }

  deleteVehicle(vehicle: VehicleListDto) {
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

  findByRegistrationNumber(event: KeyboardEvent) {
    const target: HTMLInputElement = event.target as HTMLInputElement;

    if (!target.value) {
      this.resetFilter();
      return;
    }

    this.filteredVehicleList = this.vehicleList.filter(vehicle => vehicle.registrationNumber!.toLowerCase().indexOf(target.value.toLowerCase()) > -1);
  }

  resetFilter() {
    this.filteredVehicleList = this.vehicleList;
  }
}
