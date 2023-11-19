import { HttpClient, HttpErrorResponse, HttpEvent, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, map, of } from 'rxjs';
import { DataResponse } from 'src/app/models/dataResponse';
import { EditRouteResponse, RouteListResponse, SelectedRouteResponse } from 'src/app/models/routes/routeResponse';
import { RouteInfoModel } from 'src/app/models/routes/routeInfo';
import { environment } from 'src/environments/environment';
import { SelectedRoute } from 'src/app/models/routes/selectedRoute';
import { CreateRouteModel, EditRouteModel } from 'src/app/models/routes/routeRequest';

@Injectable({
  providedIn: 'root'
})
export class RouteService {

  private url: string = environment.apiUrl;
  private selectedRoute?: SelectedRoute;
  private routeInformation?: RouteInfoModel;

  constructor(private httpClient: HttpClient, private router: Router) { }

  public getRoute(routeId: string): Observable<EditRouteResponse> {
    return this.httpClient.get<DataResponse<EditRouteResponse>>(`${this.url}Admin/Routes/Get/${routeId}`)
      .pipe(
        map((response: DataResponse<EditRouteResponse>) => response.data)
      );
  }

  public getAllRoutes(includeRoutesWithMalfunctions: boolean = false, includeInactive: boolean = false): Observable<RouteListResponse[]> {
    return this.httpClient.get<DataResponse<RouteListResponse[]>>(
      `${this.url}Routes/All?includeRoutesWithMalfunctions=${includeRoutesWithMalfunctions}&includeInactive=${includeInactive}`)
      .pipe(
        map((response: DataResponse<RouteListResponse[]>) => response.data)
      )
  }

  public deleteRoute(route: RouteListResponse): Observable<unknown> {
    return this.httpClient.put<unknown>(`${this.url}Admin/Routes/Delete/${route.id}`, null);
  }

  public editRoute(route: EditRouteModel): Observable<unknown> {
    return this.httpClient.put<unknown>(`${this.url}Admin/Routes/Edit/${route.id}`, route);
  }

  public createRoutes(routeList: CreateRouteModel[]): Observable<unknown> {
    return this.httpClient.post<unknown>(`${this.url}Admin/Routes/Create`, routeList);
  }

  public getSelectedRouteList(): Observable<SelectedRouteResponse[]> {
    const date: string = this.getDate();

    return this.httpClient
      .get<DataResponse<SelectedRouteResponse[]>>(`${this.url}Routes/GetByDate?startStationId=${this.routeInformation?.startingStation.id}&endStationId=${this.routeInformation?.endingStation.id}&date=${date}`)
      .pipe(
        map((response: DataResponse<SelectedRouteResponse[]>) => response.data),
        catchError((err: HttpEvent<unknown>) => {
          if (err instanceof HttpErrorResponse) {
            const error = err as HttpResponse<any>;

            if (error.status == HttpStatusCode.NotFound) {
              this.router.navigateByUrl('');
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

    return new Date(date).toDateString();
  }

  public setSelectedRoute(selectedRoute: SelectedRoute) {
    this.selectedRoute = selectedRoute;
  }

  public getSelectedRoute(): SelectedRoute | undefined {
    return this.selectedRoute;
  }

  public setRouteInformation(routeInfo: RouteInfoModel) {
    this.routeInformation = routeInfo;
  }

  public getRouteInformation(): RouteInfoModel | undefined {
    return this.routeInformation;
  }

}
