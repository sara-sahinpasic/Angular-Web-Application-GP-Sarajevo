import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { DelayCreateModel } from 'src/app/models/Driver/Delay/DelayCreateModel';
import { RouteDto } from 'src/app/models/Driver/Delay/RouteDto';
import { MalfunctionCreateModel } from 'src/app/models/Driver/Malfunction/MalfunctionCreateModel';
import { VehicleMalfunctionModel } from 'src/app/models/Driver/Malfunction/VehicleMalfunctionModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DriverDelayService {
  constructor(protected httpClient: HttpClient) {}
  private apiUrl: string = environment.apiUrl;

  //delay:
  getAllRoutes(): Observable<DataResponse<RouteDto[]>> {
    return this.httpClient.get<DataResponse<RouteDto[]>>(
      this.apiUrl + 'Routes'
    );
  }

  addDelay(
    delay: DelayCreateModel
  ): Observable<DataResponse<DelayCreateModel[]>> {
    return this.httpClient.post<DataResponse<DelayCreateModel[]>>(
      this.apiUrl + 'Delay',
      delay
    );
  }
  //malfunction
  getAllVehicles(): Observable<DataResponse<VehicleMalfunctionModel[]>> {
    return this.httpClient.get<DataResponse<VehicleMalfunctionModel[]>>(
      this.apiUrl + 'Vehicles'
    );
  }

  addMalfunction(
    malfunction: MalfunctionCreateModel
  ): Observable<DataResponse<unknown>> {
    console.log(malfunction)
    return this.httpClient.post<DataResponse<unknown>>(
      this.apiUrl + 'Malfunction',
      malfunction
    );
  }
}
