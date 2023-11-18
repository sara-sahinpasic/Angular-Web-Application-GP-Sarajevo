import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { tap } from 'rxjs';
import { VehicleListDto } from 'src/app/models/admin/vehicle/vehicleDto';
import { StationModel } from 'src/app/models/stations/stationModel';
import { AdminVehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';
import { CacheService } from 'src/app/services/cache/cache.service';
import { StationService } from 'src/app/services/stations/station.service';

@Component({
  selector: 'app-routes-form',
  templateUrl: './routes-form.component.html',
  styleUrls: ['./routes-form.component.scss']
})
export class RoutesFormComponent implements OnInit {
  @Input() public route?: FormGroup;
  @Input() public isCreationForm: boolean = true;

  protected stations: StationModel[] = [];
  protected vehicleList: VehicleListDto[] = [];

  constructor(
    private cacheService: CacheService,
    private stationService: StationService,
    private vehicleService: AdminVehicleService
  ) {}

  ngOnInit() {
    if (!this.route) {
      throw new Error('Route cannot be undefiend');
    }

    this.loadVehicleList();
    this.loadStations();
  }

  private loadVehicleList() {
    if (this.cacheService.getDataFromCache('routes.vehicleList')) {
      this.vehicleList = this.cacheService.getDataFromCache('routes.vehicleList') as VehicleListDto[];
      return;
    }

    this.vehicleService.getVehicleList()
      .pipe(
        tap((response: VehicleListDto[]) => {
          this.vehicleList = response;
          this.cacheService.setCacheData('routes.vehicleList', response);
        })
      )
      .subscribe();
  }

  private loadStations() {
    if (this.cacheService.getDataFromCache('routes.routeStations')) {
      this.stations = this.cacheService.getDataFromCache('routes.routeStations') as StationModel[];
      return;
    }

    this.stationService.getAllStations()
      .pipe(
        tap((stations: StationModel[]) => {
          this.stations = stations;
          this.cacheService.setCacheData('routes.routeStations', stations);
        })
      )
      .subscribe();
  }
}
