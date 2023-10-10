import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { DelayCreateModel } from 'src/app/models/Driver/Delay/DelayCreateModel';
import { RouteDto } from 'src/app/models/Driver/Delay/RouteDto';
import { DriverDelayService } from 'src/app/services/driver/driver.service';
@Component({
  selector: 'app-driver-delay-page',
  templateUrl: './driver-delay-page.component.html',
  styleUrls: ['./driver-delay-page.component.scss'],
})
export class DriverDelayPageComponent implements OnInit {
  constructor(
    protected driverService: DriverDelayService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.getAllRoutes();
    this.initForm();
  }
  registrationForm!: FormGroup;

  protected delayModel: DelayCreateModel = {};
  protected routes: Array<RouteDto> = [];

  addDelay() {
    this.registrationForm.markAllAsTouched();
    if (this.registrationForm.valid) {
      this.driverService.addDelay(this.delayModel).subscribe(() => {
        setTimeout(function () {
          window.location.reload();
        }, 3000);
      });
    }
  }

  getAllRoutes() {
    this.driverService
      .getAllRoutes()
      .pipe(
        tap((response: DataResponse<RouteDto[]>) => {
          this.routes = response.data;
          this.delayModel.routeId = response.data[0].id;
        })
      )
      .subscribe();
  }
  initForm() {
    this.registrationForm = this.formBuilder.group(
      {
        reason: ['', Validators.required],
        delayAmount: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }
}
