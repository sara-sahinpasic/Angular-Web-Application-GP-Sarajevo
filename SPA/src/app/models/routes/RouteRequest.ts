export interface CreateRouteModel {
  startStationId: string,
  endStationId: string,
  timeOfDeparture: string,
  timeOfArrival: string,
  vehicleId: string,
  activeOnHolidays: boolean,
  activeOnWeekends: boolean,
  active: boolean,
}

export interface EditRouteModel extends CreateRouteModel {
  id?: string,
}
