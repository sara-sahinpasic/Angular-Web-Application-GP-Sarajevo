import { Component, Input, OnInit } from '@angular/core';
import { tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/invalidArgumentException';
import { VehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';

@Component({
  selector: 'app-admin-vehicle-malfunction-details',
  templateUrl: './admin-vehicle-malfunction-details.component.html',
  styleUrls: ['./admin-vehicle-malfunction-details.component.scss']
})
export class AdminVehicleMalfunctionDetailsComponent implements OnInit {
  @Input()
  public vehicleId: string = '';
  protected isFixed: boolean = false;
  protected description: string = '';

  constructor(private vehicleService: VehicleService) {}

  ngOnInit() {
    if (!this.vehicleId) {
      throw new InvalidArgumentException(['vehicleId'])
    }

    this.vehicleService.getVehicleMalfunctionDetails(this.vehicleId)
      .pipe(
        tap((response: string) => this.description = response)
      )
      .subscribe();
  }

  protected markAsFixed() {
    if (!this.isFixed) {
      return;
    }

    this.vehicleService.markVehicleAsFixed(this.vehicleId)
      .subscribe(this.reoladPage);
  }

  private reoladPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }
}
