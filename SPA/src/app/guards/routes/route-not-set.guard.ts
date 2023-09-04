import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RouteService } from 'src/app/services/routes/route.service';

@Injectable({
  providedIn: 'root'
})
export class RouteNotSetGuard implements CanActivate {

  constructor(private routeService: RouteService, private router: Router) {}

  canActivate(): boolean {
    if (!!!this.routeService.getRouteInformation()) {
      this.router.navigateByUrl("");
    }

    return !!this.routeService.getRouteInformation();
  }

}
