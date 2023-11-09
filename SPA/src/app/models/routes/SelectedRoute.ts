import { StationModel } from "../stations/stationModel";
import { RouteDateModel } from "./RouteDateModel";

export interface SelectedRoute {
  id: string,
  startStation: StationModel,
  endStation: StationModel,
  dateStamp: RouteDateModel
}
