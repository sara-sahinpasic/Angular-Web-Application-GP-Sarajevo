import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { StationResponse } from 'src/app/models/stations/StationResponse';
import { environment } from 'src/environments/environment';
import { CacheService } from '../cache/cache.service';

@Injectable({
  providedIn: 'root'
})
export class StationService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient, private cacheService: CacheService) { }

  public getAllStations(): Observable<Array<StationResponse>> {
    const cachedData: Array<StationResponse> | null = this.cacheService.getDataFromCache("startingStations");

    // it is ok to cache all station data for now as we will be working with lean datasets and we won't have a lot of stations
    if (cachedData) {
      return of(cachedData);
    }

    return this.httpClient.get<DataResponse<Array<StationResponse>>>(`${this.url}Station/GetAll`)
      .pipe(
        tap((response: DataResponse<Array<StationResponse>>) => this.cacheService.setCacheData("startingStations", response.data)),
        map((response: DataResponse<Array<StationResponse>>) => response.data)
      );
  }

  public getAllRoutedStations(startingStationId: string): Observable<Array<StationResponse>> {
    const cachedData: any = this.cacheService.getDataFromCache("endingStations");

    if (cachedData && cachedData[startingStationId]) {
      return of(cachedData[startingStationId]);
    }

    return this.httpClient.get<DataResponse<Array<StationResponse>>>(`${this.url}Station/GetRouted?startStationId=${startingStationId}`)
    .pipe(
      tap((response: DataResponse<StationResponse[]>) => this.setEndStationCache(response, startingStationId)),
      map((response: DataResponse<Array<StationResponse>>) => response.data)
    );
  }

  private setEndStationCache(response: DataResponse<Array<StationResponse>>, startingStationId: string) {
    let stationsCache: any = this.cacheService.getDataFromCache("endingStations");

    if (stationsCache && !stationsCache[startingStationId]) {
      stationsCache[startingStationId] = response.data;
      localStorage.setItem("endingStations", JSON.stringify(stationsCache));

      return;
    }

    if (!stationsCache) {
      stationsCache = {
        [startingStationId]: response.data
      };

      this.cacheService.setCacheData("endingStations", stationsCache);

      return;
    }
  }
}
