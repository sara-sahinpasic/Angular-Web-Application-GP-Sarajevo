import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { HolidayCreateModel, HolidayEditModel } from 'src/app/models/holiday/holidayModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HolidayService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  public getHolidayList(): Observable<HolidayEditModel[]> {
    return this.httpClient.get<DataResponse<HolidayEditModel[]>>(`${this.url}Holiday/Get/All`)
      .pipe(
        map((response: DataResponse<HolidayEditModel[]>) => response.data)
      );
  }

  public createHoliday(holidayModel: HolidayCreateModel): Observable<unknown> {
    return this.httpClient.post<unknown>(`${this.url}Holiday/Create`, holidayModel);
  }

  public editHoliday(holidayModel: HolidayEditModel): Observable<unknown> {
    return this.httpClient.put<unknown>(`${this.url}Holiday/Edit/${holidayModel.id}`, holidayModel);
  }

  public deleteHoliday(holidayId: string): Observable<unknown> {
    return this.httpClient.delete<unknown>(`${this.url}Holiday/Delete/${holidayId}`);
  }
}
