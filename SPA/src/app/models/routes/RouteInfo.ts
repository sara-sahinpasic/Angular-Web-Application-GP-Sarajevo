import { StationResponse } from "../stations/StationResponse";
import { RouteDateModel } from "./RouteDateModel";

export interface RouteInfo {
  startingStation: StationResponse,
  endingStation: StationResponse,
  dateStamp: RouteDateModel
}
