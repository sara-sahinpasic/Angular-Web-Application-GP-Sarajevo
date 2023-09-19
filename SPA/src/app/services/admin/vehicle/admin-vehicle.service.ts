import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ManufacturerDto } from 'src/app/models/Admin/Vehicle/ManufacturerDto';
import { VehicleDto } from 'src/app/models/Admin/Vehicle/VehicleDto';
import { VehicleTypeDto } from 'src/app/models/Admin/Vehicle/VehicleTypeDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AdminVehicleService {
  constructor(protected httpClient: HttpClient) {}

  private apiUrl: string = environment.apiUrl;

  getAllManufacturers(): Observable<DataResponse<ManufacturerDto[]>> {
    return this.httpClient.get<DataResponse<ManufacturerDto[]>>(
      this.apiUrl + 'Manufacturers'
    );
  }
  getAllVehicleTypes(): Observable<DataResponse<VehicleTypeDto[]>> {
    return this.httpClient.get<DataResponse<VehicleTypeDto[]>>(
      this.apiUrl + 'VehicleType'
    );
  }
  addNewVehicle(
    vehicle: VehicleDto
  ): Observable<DataResponse<VehicleDto[]>> {
    return this.httpClient.post<DataResponse<VehicleDto[]>>(
      this.apiUrl + 'Vehicle',
      vehicle
    );
  }
}
