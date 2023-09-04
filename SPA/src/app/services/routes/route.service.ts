import { HttpClient, HttpErrorResponse, HttpEvent, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, map, of } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { RouteResponse } from 'src/app/models/routes/RouteResponse';
import { RouteInfo } from 'src/app/models/routes/RouteInfo';
import { environment } from 'src/environments/environment';
import { SelectedRoute } from 'src/app/models/routes/SelectedRoute';

@Injectable({
  providedIn: 'root'
})
export class RouteService {

  private url: string = environment.apiUrl;
  private selectedRoute?: SelectedRoute;
  private routeInformation?: RouteInfo;

  constructor(private httpClient: HttpClient, private router: Router) { }

  public getRoutes(): Observable<RouteResponse[]> {
    const date: string = this.getDate();

    return this.httpClient
      .get<DataResponse<RouteResponse[]>>(`${this.url}Routes/GetByDate?startStationId=${this.routeInformation?.startingStation.id}&endStationId=${this.routeInformation?.endingStation.id}&date=${date}`)
      .pipe(
        map((response: DataResponse<RouteResponse[]>) => response.data),
        catchError((err: HttpEvent<unknown>) => {
          if (err instanceof HttpErrorResponse) {
            const error = err as HttpResponse<any>;

            if (error.status == HttpStatusCode.NotFound) {
              this.router.navigateByUrl("");
            }
          }

          return of([]);
        })
      );
  }

  private getDate(): string {
    let date: string = this.routeInformation?.dateStamp.date as string;

    if (this.routeInformation?.dateStamp.time) {

      date += ` ${this.routeInformation.dateStamp.time}`;
    }

    return new Date(date).toUTCString();
  }

  public setSelectedRoute(selectedRoute: SelectedRoute) {
    this.selectedRoute = selectedRoute;
  }

  public getSelectedRoute(): SelectedRoute | undefined {
    return this.selectedRoute;
  }

  public setRouteInformation(routeInfo: RouteInfo) {
    this.routeInformation = routeInfo;
  }

  public getRouteInformation(): RouteInfo | undefined {
    return this.routeInformation;
  }

}
