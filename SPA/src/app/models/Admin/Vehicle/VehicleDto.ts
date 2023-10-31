import { ManufacturerDto } from "./ManufacturerDto";
import { VehicleTypeDto } from "./VehicleTypeDto";

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
  manufacturer: ManufacturerDto,
  type: VehicleTypeDto
}
