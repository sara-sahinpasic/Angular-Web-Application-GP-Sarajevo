import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { VehicleListDto } from 'src/app/models/admin/vehicle/vehicleDto';
import { MalfunctionCreateModel } from 'src/app/models/driver/malfunction/malfunctionCreateModel';
import { VehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';
import { DriverService } from 'src/app/services/driver/driver.service';

@Component({
  selector: 'app-driver-malfunction-page',
  templateUrl: './driver-malfunction-page.component.html',
  styleUrls: ['./driver-malfunction-page.component.scss'],
})
export class DriverMalfunctionPageComponent implements OnInit {
  protected malfunctionForm: FormGroup = {} as FormGroup;
  protected malfunctionModel: MalfunctionCreateModel = {
    dateOfMalufunction: new Date(),
  };
  protected vehicles: VehicleListDto[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private driverService: DriverService,
    private vehicleService: VehicleService
  ) {}

  ngOnInit() {
    this.initializeForm();
    this.getAllVehicles();
  }

  private initializeForm() {
    this.malfunctionForm = this.formBuilder.group(
      {
        description: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }

  protected addMalfunction() {
    this.malfunctionForm.markAllAsTouched();
    if (this.malfunctionForm.valid) {
      this.driverService.addMalfunction(this.malfunctionModel)
        .subscribe(this.reloadPage);
    }
  }

  private reloadPage() {
    setTimeout(function () {
      window.location.reload();
    }, 1500);
  }

  protected getAllVehicles() {
    this.vehicleService
      .getVehicleList()
      .pipe(
        tap((response: VehicleListDto[]) => {
          this.vehicles = response;
          this.malfunctionModel.vehicleId = response[0].id;
        })
      )
      .subscribe();
  }
}
