import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { SelectedRouteResponse } from 'src/app/models/routes/routeResponse';
import { RouteInfoModel } from 'src/app/models/routes/routeInfo';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { RouteService } from 'src/app/services/routes/route.service';
import { SelectedRoute } from 'src/app/models/routes/selectedRoute';

@Component({
  selector: 'app-routes',
  templateUrl: './routes.component.html',
  styleUrls: ['./routes.component.scss']
})
export class RoutesComponent implements OnInit{
  protected routeList: SelectedRouteResponse[] = [];

  constructor(
    protected localizationService: LocalizationService,
    private routeService: RouteService,
    private router: Router) {}

  ngOnInit() {
    this.routeService.getSelectedRouteList()
      .pipe(
        tap((data: SelectedRouteResponse[]) => this.routeList = data)
      )
      .subscribe();
  }

  // we have guards that guard agains undefined selected routes so the selectedRoute variable should never be undefined
  protected chooseRouteTime(route: SelectedRouteResponse) {
    const routeInfo: RouteInfoModel = this.routeService.getRouteInformation()!;
    const selectedRoute: SelectedRoute = {
      id: route.id,
      dateStamp: {
        date: route.date,
        time: route.time
      },
      endStation: routeInfo.endingStation,
      startStation: routeInfo.startingStation
    };

    this.routeService.setSelectedRoute(selectedRoute);
    this.router.navigateByUrl('/checkout');
  }
}
