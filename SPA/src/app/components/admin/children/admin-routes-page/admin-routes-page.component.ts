import { Component, OnInit } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { Pagination } from 'src/app/models/pagination/pagination';
import { RouteListResponse } from 'src/app/models/routes/routeResponse';
import { ModalService } from 'src/app/services/modal/modal.service';
import { RouteService } from 'src/app/services/routes/route.service';

@Component({
  selector: 'app-admin-routes-page',
  templateUrl: './admin-routes-page.component.html',
  styleUrls: ['./admin-routes-page.component.scss']
})
export class AdminRoutesPageComponent implements OnInit {
  public paginationModel: Pagination = {
    pageSize: 5,
    page: 1,
    totalCount: 0,
  };

  protected routesList: RouteListResponse[] = [];
  protected filteredRoutesList: RouteListResponse[] = [];
  protected editIcon = faEdit;
  protected deleteIcon = faTrash;

  constructor(private routeService: RouteService, private modalService: ModalService) {}

  ngOnInit() {
    this.getRouteData();
  }

  private getRouteData() {
    const includeRoutesWithMalfunctions: boolean = true;
    const includeInactiveRoutes: boolean = true;

    this.routeService.getAllRoutes(includeRoutesWithMalfunctions, includeInactiveRoutes)
      .pipe(
        tap((response: RouteListResponse[]) => {
          this.routesList = response;
          this.filteredRoutesList = response;
        })
      )
      .subscribe();
  }

  protected findByStationName(startStation: string, endStation: string) {
    let tmpList: RouteListResponse[] = this.routesList;

    if (!startStation && !endStation) {
      this.resetFilter();
      return;
    }

    if (startStation) {
      tmpList = tmpList.filter(route =>
        route.startingLocation.toLowerCase()
          .indexOf(startStation.toLowerCase()) > -1
      );
    }

    if (endStation) {
      tmpList = tmpList.filter(route =>
        route.endingLocation.toLowerCase()
          .indexOf(endStation.toLowerCase()) > -1
      );
    }

    this.filteredRoutesList = tmpList;
  }

  private resetFilter() {
    this.filteredRoutesList = this.routesList;
  }

  protected deactivateRoute(route: RouteListResponse) {
    const isDeactivationConfirmed: boolean = confirm('Da li želite obrisati rutu?');

    if (!isDeactivationConfirmed) {
      return;
    }

    this.routeService.deleteRoute(route)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  protected showRouteEditModal(route: RouteListResponse) {
    this.modalService.data = route.id;
    this.modalService.adminShowEditRouteModal();
  }

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }
}
