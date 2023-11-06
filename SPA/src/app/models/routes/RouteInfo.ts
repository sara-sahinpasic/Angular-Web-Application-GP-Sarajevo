import { StationResponse } from "../stations/StationResponse";
import { RouteDateModel } from "./RouteDateModel";

export interface RouteInfoModel {
  startingStation: StationResponse,
  endingStation: StationResponse,
  dateStamp: RouteDateModel
}
