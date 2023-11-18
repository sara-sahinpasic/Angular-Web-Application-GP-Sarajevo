import { StationModel } from "../stations/stationModel";
import { RouteDateModel } from "./routeDateModel";

export interface RouteInfoModel {
  startingStation: StationModel,
  endingStation: StationModel,
  dateStamp: RouteDateModel
}
