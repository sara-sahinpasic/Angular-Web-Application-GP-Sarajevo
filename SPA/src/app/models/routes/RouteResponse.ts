interface RouteResponseBase {
  id: string,
  startingLocation: string,
  endingLocation: string,
}

export interface SelectedRouteResponse extends RouteResponseBase {
  time: string,
  date: string
}

export interface RouteListResponse extends RouteResponseBase {
  timeOfDeparture: string,
  timeOfArrival: string,
  vehicleNumber: number,
  activeOnHolidays: boolean,
  activeOnWeekends: boolean,
  active: boolean
}

export interface EditRouteResponse {
  id: string,
  startStationId: string,
  endStationId: string,
  timeOfDeparture: string,
  timeOfArrival: string,
  vehicleId: string,
  activeOnHolidays: boolean,
  activeOnWeekends: boolean,
  active: boolean
}
