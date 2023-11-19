import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ManufacturerDto } from 'src/app/models/admin/vehicle/manufacturerDto';
import { VehicleDto, VehicleListDto } from 'src/app/models/admin/vehicle/vehicleDto';
import { VehicleTypeDto } from 'src/app/models/admin/vehicle/vehicleTypeDto';
import { DataResponse } from 'src/app/models/dataResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  constructor(protected httpClient: HttpClient) {}

  private apiUrl: string = environment.apiUrl;

  public getAllManufacturers(): Observable<DataResponse<ManufacturerDto[]>> {
    return this.httpClient.get<DataResponse<ManufacturerDto[]>>(
      this.apiUrl + 'Vehicles/Manufacturers/All'
    );
  }

  public getAllVehicleTypes(): Observable<DataResponse<VehicleTypeDto[]>> {
    return this.httpClient.get<DataResponse<VehicleTypeDto[]>>(
      this.apiUrl + 'Vehicles/Types/All'
    );
  }

  public addNewVehicle(vehicle: VehicleDto): Observable<DataResponse<VehicleDto[]>> {
    return this.httpClient.post<DataResponse<VehicleDto[]>>(
      this.apiUrl + 'Vehicles/Add',
      vehicle
    );
  }

  public addNewVehicleType(vehicleType: VehicleTypeDto): Observable<DataResponse<VehicleTypeDto[]>> {
    return this.httpClient.post<DataResponse<VehicleTypeDto[]>>(
      this.apiUrl + 'Vehicles/Types/Add',
      vehicleType
    );
  }

  public addNewManufacturer(manufacturer: ManufacturerDto): Observable<DataResponse<ManufacturerDto[]>> {
    return this.httpClient.post<DataResponse<ManufacturerDto[]>>(
      this.apiUrl + 'Vehicles/Manufacturers/Add',
      manufacturer
    );
  }

  public deleteManufacturer(manufacturerId: string): Observable<unknown> {
    return this.httpClient.delete<unknown>(`${this.apiUrl}Vehicles/Manufacturers/Delete/${manufacturerId}`);
  }

  public hasVehicleAnyManufacturerDependencies(manufacturerId: string): Observable<boolean> {
    return this.httpClient.get<DataResponse<boolean>>(`${this.apiUrl}Vehicles/Manufacturers/HasDependentVehicles/${manufacturerId}`)
      .pipe(
        map((response: DataResponse<boolean>) => response.data)
      );
  }

  public getVehicleList(): Observable<VehicleListDto[]> {
    return this.httpClient.get<DataResponse<VehicleListDto[]>>(`${this.apiUrl}Vehicles/All`)
      .pipe(
        map((response: DataResponse<VehicleListDto[]>) => response.data)
      );
  }

  public deleteVehicle(vehicle: VehicleListDto): Observable<unknown> {
    return this.httpClient.delete<unknown>(`${this.apiUrl}Vehicles/Delete/${vehicle.id}`);
  }

  public deleteVehicleType(vehicleTypeId: string): Observable<unknown> {
    return this.httpClient.delete<unknown>(`${this.apiUrl}Vehicles/Types/Delete/${vehicleTypeId}`);
  }

  public hasVehicleAnyVehicleTypeDependencies(vehiceTypeId: string): Observable<boolean> {
    return this.httpClient.get<DataResponse<boolean>>(`${this.apiUrl}Vehicles/Types/HasDependentVehicles/${vehiceTypeId}`)
      .pipe(
        map((response: DataResponse<boolean>) => response.data)
      );
  }

  public editVehicle(vehicle: VehicleDto): Observable<unknown> {
    return this.httpClient.put<unknown>(`${this.apiUrl}Vehicles/Edit/${vehicle.id}`, vehicle);
  }

  public getVehicleMalfunctionDetails(vehicleId: string): Observable<string> {
    return this.httpClient.get<DataResponse<string>>(`${this.apiUrl}Vehicles/Malfunctions/Get/${vehicleId}`)
      .pipe(
        map((response: DataResponse<string>) => response.data)
      );
  }

  public markVehicleAsFixed(vehicleId: string): Observable<unknown> {
    return this.httpClient.put<unknown>(`${this.apiUrl}Vehicles/Malfunctions/MarkAsFixed/${vehicleId}`, null);
  }
}
