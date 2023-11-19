import { ManufacturerDto } from "./manufacturerDto";
import { VehicleTypeDto } from "./vehicleTypeDto";

interface VehicleDtoBase {
  id?: string,
  number?: number;
  registrationNumber?: string;
  color?: string;
  buildYear?: number;
}

export interface VehicleDto extends VehicleDtoBase {
  manufacturerId?: string;
  vehicleTypeId?: string;
}

export interface VehicleListDto extends VehicleDtoBase {
  manufacturer: ManufacturerDto;
  type: VehicleTypeDto;
  hasMalfunction: boolean;
}
