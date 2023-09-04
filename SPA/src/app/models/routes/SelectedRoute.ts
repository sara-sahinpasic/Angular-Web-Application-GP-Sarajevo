import { StationResponse } from "../stations/StationResponse";
import { RouteDateModel } from "./RouteDateModel";

export interface SelectedRoute {
  id: string,
  startStation: StationResponse,
  endStation: StationResponse,
  dateStamp: RouteDateModel
}
