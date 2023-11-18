import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/invalidArgumentException';
import { CreateRouteModel, EditRouteModel } from 'src/app/models/routes/routeRequest';
import { EditRouteResponse } from 'src/app/models/routes/routeResponse';
import { StationModel } from 'src/app/models/stations/stationModel';
import { RouteService } from 'src/app/services/routes/route.service';

@Component({
  selector: 'app-admin-route-edit-modal',
  templateUrl: './admin-route-edit-modal.component.html',
  styleUrls: ['./admin-route-edit-modal.component.scss']
})
export class AdminRouteEditModalComponent implements OnInit {
  @Input() routeId?: string;

  protected routeModel!: CreateRouteModel;
  protected routeEditForm!: FormGroup;
  protected stations: StationModel[] = [];

  constructor(
    private routeService: RouteService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    if (!this.routeId) {
      throw new InvalidArgumentException(['routeId']);
    }

    setTimeout(this.getRouteData.bind(this), 0);
  }

  private getRouteData() {
    this.routeService.getRoute(this.routeId!)
      .pipe(
        tap(this.initializeForm.bind(this))
      )
      .subscribe();
  }

  protected editRoute() {
    this.validateForm();

    if (this.routeEditForm.invalid) {
      return;
    }

    const editRouteModel: EditRouteModel = this.routeEditForm.value as EditRouteModel;
    editRouteModel.id = this.routeId;

    this.routeService.editRoute(editRouteModel)
      .subscribe(this.reloadPage);
  }

  private validateForm() {
      Object.keys(this.routeEditForm.controls).forEach((controlName: string) => {
        this.routeEditForm.controls[controlName].updateValueAndValidity();
        this.routeEditForm.controls[controlName].markAsTouched();
      });
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  protected initializeForm(route: EditRouteResponse) {
    this.routeEditForm = this.formBuilder.group({
      active: [route.active, null],
      activeOnHolidays: [route.activeOnHolidays, null],
      activeOnWeekends: [route.activeOnWeekends, null],
      startStationId: [route.startStationId, Validators.compose([Validators.required, this.validateRoutedStations])],
      endStationId: [route.endStationId, Validators.compose([Validators.required, this.validateRoutedStations])],
      timeOfArrival: [route.timeOfArrival, Validators.compose([Validators.required, this.validateTime])],
      timeOfDeparture: [route.timeOfDeparture, Validators.compose([Validators.required, this.validateTime])],
      vehicleId: [route.vehicleId, Validators.required],
    });
  }

  private validateRoutedStations(control: AbstractControl | null): ValidationErrors | null {
    const parentFormGroup: FormGroup | FormArray | undefined | null = control?.parent;

    if (!parentFormGroup) {
      return null;
    }

    const startStationValue: string | undefined = parentFormGroup.get('startStationId')?.value;
    const endStationValue: string | undefined = parentFormGroup.get('endStationId')?.value;

    if (startStationValue !== endStationValue) {
      return null;
    }

    return {
      notUnique: true
    };
  }

  private validateTime(control: AbstractControl | null): ValidationErrors | null {
    const parentFormGroup: FormGroup | FormArray | undefined | null = control?.parent;

    if (!parentFormGroup) {
      return null;
    }

    const timeOfArrival: string | undefined = parentFormGroup.get('timeOfArrival')?.value;
    const timeOfDeparture: string | undefined = parentFormGroup.get('timeOfDeparture')?.value;

    if (timeOfArrival !== timeOfDeparture) {
      return null;
    }

    return {
      invalidTimeConfiguration: true
    };
  }
}
