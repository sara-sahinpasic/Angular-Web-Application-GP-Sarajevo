import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { RouteDateModel } from 'src/app/models/routes/RouteDateModel';
import { RouteInfo } from 'src/app/models/routes/RouteInfo';
import { StationResponse } from 'src/app/models/stations/StationResponse';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { RouteService } from 'src/app/services/routes/route.service';
import { StationService } from 'src/app/services/stations/station.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit{

  protected startingStations: Array<StationResponse> = [];
  protected endingStations: Array<StationResponse> = [];
  protected formGroup!: FormGroup;
  protected routeDate: RouteDateModel = {
    time: null,
    date: ""
  };
  private startingStation?: StationResponse;

  constructor(
    protected localizationService: LocalizationService,
    private stationService: StationService,
    private formBuilder: FormBuilder,
    private routeService: RouteService,
    private router: Router) { }

  ngOnInit() {
    this.initializeValidators();

    this.stationService.getAllStations()
      .pipe(
        tap(this.setStartingStations.bind(this))
      )
      .subscribe();
  }

  private setStartingStations(data: Array<StationResponse>) {
    this.startingStations = data;
  }

  private initializeValidators() {
    this.formGroup = this.formBuilder.group({
      start: ["", (control: FormControl) => this.validateStation(control, this.startingStations, "start")],
      end: ["", (control: FormControl) => this.validateStation(control, this.endingStations, "end")],
    });
  }

  private validateStation(control: FormControl, stations: Array<StationResponse>, controlName: string): object | null {
    if (stations.findIndex((station: StationResponse) => station.name == control.value) > -1) {
      return null;
    }
    return {
      [controlName]: false
    };
  }

  protected initDesitnationStations() {
   this.setStartingStation(this.formGroup.get("start")?.value)

    if (!this.startingStation) {
      this.endingStations = [];
      return;
    }

    this.stationService.getAllRoutedStations(this.startingStation.id)
      .pipe(
        tap((data: Array<StationResponse>) => this.endingStations = data)
      )
      .subscribe();
  }

  private setStartingStation(stationName: string) {
    this.startingStation = this.startingStations.find((station: StationResponse) => station.name === stationName);
  }

  // todo: Guard for the routes route. Fix for routing to home not workin issue.

  protected search() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    const endingStation: StationResponse = this.endingStations.find((station: StationResponse) => station.name === this.formGroup.get("end")?.value)!;

    if (!this.routeDate.date) {
      this.routeDate.date = new Date().toISOString().split("T")[0];
    }

    const routeInfo: RouteInfo = {
      startingStation: this.startingStation!,
      endingStation: endingStation,
      dateStamp: this.routeDate
    };

    this.routeService.setRouteInformation(routeInfo);
    this.router.navigateByUrl("/routes");
  }
}
