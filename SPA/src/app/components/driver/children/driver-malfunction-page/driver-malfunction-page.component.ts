import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { MalfunctionCreateModel } from 'src/app/models/Driver/Malfunction/MalfunctionCreateModel';
import { VehicleMalfunctionModel } from 'src/app/models/Driver/Malfunction/VehicleMalfunctionModel';
import { DriverDelayService } from 'src/app/services/driver/driver.service';

@Component({
  selector: 'app-driver-malfunction-page',
  templateUrl: './driver-malfunction-page.component.html',
  styleUrls: ['./driver-malfunction-page.component.scss'],
})
export class DriverMalfunctionPageComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    protected driverService: DriverDelayService
  ) {}
  ngOnInit(): void {
    this.initForm();
    this.getAllVehicles();
  }
  registrationForm!: FormGroup;
  protected malfunctionModel: MalfunctionCreateModel = {
    dateOfMalufunction: new Date(),
  };

  protected vehicles: Array<VehicleMalfunctionModel> = [];
  initForm() {
    this.registrationForm = this.formBuilder.group(
      {
        description: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }

  addMalfunction() {
    this.registrationForm.markAllAsTouched();
    if (this.registrationForm.valid) {
      this.driverService.addMalfunction(this.malfunctionModel).subscribe(() => {
        setTimeout(function () {
          window.location.reload();
        }, 3000);
      });
    }
  }
  getAllVehicles() {
    this.driverService
      .getAllVehicles()
      .pipe(
        tap((response: DataResponse<VehicleMalfunctionModel[]>) => {
          this.vehicles = response.data;
          this.malfunctionModel.vehicleId = response.data[0].id;
        })
      )
      .subscribe();
  }
}
