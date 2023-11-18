import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { CreateRouteModel } from 'src/app/models/routes/routeRequest';
import { ModalService } from 'src/app/services/modal/modal.service';
import { RouteService } from 'src/app/services/routes/route.service';

@Component({
  selector: 'app-admin-route-modal',
  templateUrl: './admin-route-modal.component.html',
  styleUrls: ['./admin-route-modal.component.scss']
})
export class AdminRouteModalComponent implements OnInit {
  protected routeList: CreateRouteModel[] = [];
  protected routeCreationForm!: FormGroup;

  constructor(
    protected routeService: RouteService,
    protected formBuilder: FormBuilder,
    protected modalService: ModalService,
  ) {}

  ngOnInit() {
    this.initializeForm();
    this.addRoute();
  }

  private get routesFormArray() {
    return this.routeCreationForm.controls['routes'] as FormArray;
  }

  protected get routeFormGroupArray() {
    return this.routesFormArray.controls as FormGroup[];
  }

  protected saveRoutes() {
    this.validateForm();

    if (this.routeCreationForm.invalid) {
      return;
    }

    this.routeService.createRoutes(this.routesFormArray.value)
      .subscribe(this.modalService.closeModal.bind(this.modalService));
  }

  private validateForm() {
    this.routeFormGroupArray.forEach((group: FormGroup) => {
      const groupControls: any = group.controls;

      Object.keys(groupControls).forEach((controlName: string) => {
        groupControls[controlName].updateValueAndValidity();
        groupControls[controlName].markAsTouched();
      });
    })
  }

  protected addRoute() {
    const newRouteFormGroup = this.formBuilder.group({
      active: [false, null],
      activeOnHolidays: [false, null],
      activeOnWeekends: [false, null],
      startStationId: ['', Validators.compose([Validators.required, this.validateRoutedStations])],
      endStationId: ['', Validators.compose([Validators.required, this.validateRoutedStations])],
      timeOfArrival: ['00:00', Validators.compose([Validators.required, this.validateTime])],
      timeOfDeparture: ['00:00', Validators.compose([Validators.required, this.validateTime])],
      vehicleId: ['', Validators.required],
    },
    {
      updateOn: 'blur'
    });

    this.routesFormArray.push(newRouteFormGroup);
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

  protected discardRoute(index: number) {
    this.routesFormArray.removeAt(index);
  }

  private initializeForm() {
    this.routeCreationForm = this.formBuilder.group({
      routes: this.formBuilder.array([])
    });
  }
}
