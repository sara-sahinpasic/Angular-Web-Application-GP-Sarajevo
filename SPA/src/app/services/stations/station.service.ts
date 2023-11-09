import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { StationModel } from 'src/app/models/stations/stationModel';
import { environment } from 'src/environments/environment';
import { CacheService } from '../cache/cache.service';

@Injectable({
  providedIn: 'root'
})
export class StationService {

  private url: string = environment.apiUrl;

  constructor(private httpClient: HttpClient, private cacheService: CacheService) { }

  public getAllStations(): Observable<Array<StationModel>> {
    const cachedData: StationModel[] | null = this.cacheService.getDataFromCache("startingStations");

    // it is ok to cache all station data for now as we will be working with lean datasets and we won't have a lot of stations
    if (cachedData) {
      return of(cachedData);
    }

    return this.httpClient.get<DataResponse<StationModel[]>>(`${this.url}Station/GetAll`)
      .pipe(
        tap((response: DataResponse<StationModel[]>) => this.cacheService.setCacheData("startingStations", response.data)),
        map((response: DataResponse<StationModel[]>) => response.data)
      );
  }

  public getAllRoutedStations(startingStationId: string): Observable<StationModel[]> {
    const cachedData: any = this.cacheService.getDataFromCache("endingStations");

    if (cachedData && cachedData[startingStationId]) {
      return of(cachedData[startingStationId]);
    }

    return this.httpClient.get<DataResponse<StationModel[]>>(`${this.url}Station/GetRouted?startStationId=${startingStationId}`)
    .pipe(
      tap((response: DataResponse<StationModel[]>) => this.setEndStationCache(response, startingStationId)),
      map((response: DataResponse<StationModel[]>) => response.data)
    );
  }

  private setEndStationCache(response: DataResponse<StationModel[]>, startingStationId: string) {
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

  public createStation(stationModel: StationModel): Observable<StationModel> {
    this.clearStationCache();

    return this.httpClient.post<DataResponse<StationModel>>(`${this.url}Station/Create`, stationModel)
      .pipe(
        map((response: DataResponse<StationModel>) => response.data)
      );
  }

  public editStation(stationModel: StationModel): Observable<unknown> {
    this.clearStationCache();

    return this.httpClient.put(`${this.url}Station/Edit/${stationModel.id}`, stationModel);
  }

  public deleteStation(stationModel: StationModel): Observable<unknown> {
    this.clearStationCache();

    return this.httpClient.delete<unknown>(`${this.url}Station/Delete/${stationModel.id}`);
  }

  private clearStationCache() {
    this.cacheService.unsetCacheItem("startingStations");
  }
}
