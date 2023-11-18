import { StationModel } from "../stations/stationModel";
import { RouteDateModel } from "./routeDateModel";

export interface SelectedRoute {
  id: string,
  startStation: StationModel,
  endStation: StationModel,
  dateStamp: RouteDateModel
}
