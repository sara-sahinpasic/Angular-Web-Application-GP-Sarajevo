import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { DelayCreateModel } from 'src/app/models/driver/delay/delayCreateModel';
import { RouteListResponse } from 'src/app/models/routes/routeResponse';
import { DriverService } from 'src/app/services/driver/driver.service';
import { RouteService } from 'src/app/services/routes/route.service';

@Component({
  selector: 'app-driver-delay-page',
  templateUrl: './driver-delay-page.component.html',
  styleUrls: ['./driver-delay-page.component.scss'],
})
export class DriverDelayPageComponent implements OnInit {
  protected delayForm: FormGroup = {} as FormGroup;
  protected delayModel: DelayCreateModel = {} as DelayCreateModel;
  protected routes: RouteListResponse[] = [];

  constructor(
    private driverService: DriverService,
    private routeService: RouteService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.getAllRoutes();
    this.initializeForm();
  }

  protected addDelay() {
    this.delayForm.markAllAsTouched();

    if (this.delayForm.valid) {
      this.driverService.addDelay(this.delayModel)
      .subscribe(this.reloadPage);
    }
  }

  private reloadPage() {
    setTimeout(function () {
      window.location.reload();
    }, 1500);
  }

  private getAllRoutes() {
    this.routeService
      .getAllRoutes()
      .pipe(
        tap((response: RouteListResponse[]) => {
          this.routes = response;
          this.delayModel.routeId = response[0].id;
        })
      )
      .subscribe();
  }

  private initializeForm() {
    this.delayForm = this.formBuilder.group(
      {
        reason: ['', Validators.required],
        delayAmount: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }
}
