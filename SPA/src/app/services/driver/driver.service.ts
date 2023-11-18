import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/dataResponse';
import { DelayCreateModel } from 'src/app/models/driver/delay/delayCreateModel';
import { MalfunctionCreateModel } from 'src/app/models/driver/malfunction/malfunctionCreateModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DriverService {
  private apiUrl: string = environment.apiUrl;

  constructor(protected httpClient: HttpClient) {}

  addDelay(delay: DelayCreateModel): Observable<DataResponse<DelayCreateModel[]>> {
    return this.httpClient.post<DataResponse<DelayCreateModel[]>>(
      this.apiUrl + 'Driver/Delay/Notify',
      delay
    );
  }

  addMalfunction(malfunction: MalfunctionCreateModel): Observable<DataResponse<unknown>> {
    return this.httpClient.post<DataResponse<unknown>>(
      this.apiUrl + 'Driver/Malfunction/Notify',
      malfunction
    );
  }
}
