import { Component, OnInit } from '@angular/core';
import { faCheck, faClose, faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { Pagination } from 'src/app/models/pagination/pagination';
import { StationListModel, StationModel } from 'src/app/models/stations/stationModel';
import { StationService } from 'src/app/services/stations/station.service';

@Component({
  selector: 'app-admin-station-modal',
  templateUrl: './admin-station-modal.component.html',
  styleUrls: ['./admin-station-modal.component.scss'],
})
export class AdminStationModalComponent implements OnInit {
  protected deleteIcon = faTrash;
  protected editIcon = faEdit;
  protected checkmarkIcon = faCheck;
  protected closeIcon = faClose;
  protected stations: StationListModel[] = [];
  protected stationCreateModel: StationModel = {} as StationModel
  protected isStationNameValid?: boolean;
  protected paginationModel: Pagination = {
    pageSize: 3,
    page: 1,
    totalCount: 0,
  };

  constructor(private stationService: StationService) {}

  ngOnInit() {
    setTimeout(() => {
      this.getAllStations();
    }, 0);
  }

  private getAllStations() {
    this.stationService.getAllStations()
      .pipe(
        tap((response: StationModel[]) => this.stations = response as StationListModel[])
      )
      .subscribe();
  }

  protected createNewStation() {
    if (!this.stationCreateModel.name) {
      this.isStationNameValid = false;
      return;
    }

    this.stationService.createStation(this.stationCreateModel)
      .pipe(
        tap((response: StationModel) => this.stations.unshift(response as StationListModel))
      )
    .subscribe();
  }

  protected deleteStation(station: StationModel) {
    this.stationService.deleteStation(station)
      .pipe(
        tap(() => this.stations = this.stations.filter((s: StationListModel) => s.id != station.id))
      )
      .subscribe();
  }

  protected startEditing(station: StationListModel) {
    station.isEditing = true;
    station.oldNameValue = station.name;
  }

  protected cancelEditing(station: StationListModel) {
    station.name = station.oldNameValue as string;
    station.isEditing = false;
  }

  protected finishEditing(station: StationListModel) {
    if (!station.name) {
      this.cancelEditing(station);
      return;
    }

    this.stationService.editStation(station)
      .pipe(
        tap(() => station.isEditing = false)
      )
      .subscribe();
  }

  protected perserveOldValue(station: StationListModel) {
    station.oldNameValue = station.name;
  }

  protected validate(target: EventTarget | null) {
    const inputElement: HTMLInputElement = target as HTMLInputElement;

    this.isStationNameValid = !!inputElement.value;
  }

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }
}
