import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { RouteResponse } from 'src/app/models/routes/RouteResponse';
import { RouteInfo } from 'src/app/models/routes/RouteInfo';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { RouteService } from 'src/app/services/routes/route.service';
import { SelectedRoute } from 'src/app/models/routes/SelectedRoute';

@Component({
  selector: 'app-routes',
  templateUrl: './routes.component.html',
  styleUrls: ['./routes.component.scss']
})
export class RoutesComponent implements OnInit{

  protected routeList: RouteResponse[] = [];

  constructor(protected localizationService: LocalizationService, private routeService: RouteService, private router: Router) {}

  ngOnInit() {
    this.routeService.getRoutes()
      .pipe(
        tap((data: RouteResponse[]) => this.routeList = data)
      )
      .subscribe();
  }

  // we have guards that guard agains undefined selected routes so the selectedRoute variable should never be undefined
  chooseRouteTime(route: RouteResponse) {
    const routeInfo: RouteInfo = this.routeService.getRouteInformation()!;
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
    this.router.navigateByUrl("/checkout");
  }
}
