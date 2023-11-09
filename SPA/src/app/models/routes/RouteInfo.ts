import { StationModel } from "../stations/stationModel";
import { RouteDateModel } from "./RouteDateModel";

export interface RouteInfoModel {
  startingStation: StationModel,
  endingStation: StationModel,
  dateStamp: RouteDateModel
}
