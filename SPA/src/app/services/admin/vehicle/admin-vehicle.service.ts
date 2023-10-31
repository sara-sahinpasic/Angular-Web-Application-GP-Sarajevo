import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ManufacturerDto } from 'src/app/models/Admin/Vehicle/ManufacturerDto';
import { VehicleDto, VehicleListDto } from 'src/app/models/Admin/Vehicle/VehicleDto';
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
      this.apiUrl + 'Vehicles/Manufacturers/All'
    );
  }

  getAllVehicleTypes(): Observable<DataResponse<VehicleTypeDto[]>> {
    return this.httpClient.get<DataResponse<VehicleTypeDto[]>>(
      this.apiUrl + 'Vehicles/Types/All'
    );
  }

  addNewVehicle(vehicle: VehicleDto): Observable<DataResponse<VehicleDto[]>> {
    return this.httpClient.post<DataResponse<VehicleDto[]>>(
      this.apiUrl + 'Vehicles/Add',
      vehicle
    );
  }

  addNewVehicleType(vehicleType: VehicleTypeDto): Observable<DataResponse<VehicleTypeDto[]>> {
    return this.httpClient.post<DataResponse<VehicleTypeDto[]>>(
      this.apiUrl + 'Vehicles/Types/Add',
      vehicleType
    );
  }

  addNewManufacturer(manufacturer: ManufacturerDto): Observable<DataResponse<ManufacturerDto[]>> {
    return this.httpClient.post<DataResponse<ManufacturerDto[]>>(
      this.apiUrl + 'Vehicles/Manufacturers/Add',
      manufacturer
    );
  }

  deleteManufacturer(manufacturerId: string): Observable<unknown> {
    return this.httpClient.delete<unknown>(`${this.apiUrl}Vehicles/Manufacturers/Delete/${manufacturerId}`);
  }

  hasVehicleAnyManufacturerDependencies(manufacturerId: string): Observable<boolean> {
    return this.httpClient.get<DataResponse<boolean>>(`${this.apiUrl}Vehicles/Manufacturers/HasDependentVehicles/${manufacturerId}`)
      .pipe(
        map((response: DataResponse<boolean>) => response.data)
      );
  }

  getVehicleList(): Observable<VehicleListDto[]> {
    return this.httpClient.get<DataResponse<VehicleListDto[]>>(`${this.apiUrl}Vehicles/All`)
      .pipe(
        map((response: DataResponse<VehicleListDto[]>) => response.data)
      );
  }

  deleteVehicle(vehicle: VehicleListDto): Observable<unknown> {
    return this.httpClient.delete<unknown>(`${this.apiUrl}Vehicles/Delete/${vehicle.id}`);
  }

  deleteVehicleType(vehicleTypeId: string): Observable<unknown> {
    return this.httpClient.delete<unknown>(`${this.apiUrl}Vehicles/Types/Delete/${vehicleTypeId}`);
  }


  hasVehicleAnyVehicleTypeDependencies(vehiceTypeId: string): Observable<boolean> {
    return this.httpClient.get<DataResponse<boolean>>(`${this.apiUrl}Vehicles/Types/HasDependentVehicles/${vehiceTypeId}`)
      .pipe(
        map((response: DataResponse<boolean>) => response.data)
      );
  }

  editVehicle(vehicle: VehicleDto): Observable<unknown> {
    return this.httpClient.put<unknown>(`${this.apiUrl}Vehicles/Edit/${vehicle.id}`, vehicle);
  }
}
