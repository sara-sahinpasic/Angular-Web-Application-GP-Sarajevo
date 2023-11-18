import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { ManufacturerDto } from 'src/app/models/admin/vehicle/manufacturerDto';
import { VehicleListDto, VehicleDto } from 'src/app/models/admin/vehicle/vehicleDto';
import { VehicleTypeDto } from 'src/app/models/admin/vehicle/vehicleTypeDto';
import { DataResponse } from 'src/app/models/dataResponse';
import { AdminVehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-vehicle-modal',
  templateUrl: './admin-vehicle-modal.component.html',
  styleUrls: ['./admin-vehicle-modal.component.scss'],
})
export class AdminVehicleModalComponent implements OnInit {
  @Input() vehicle?: VehicleListDto;

  protected vehicleForm!: FormGroup;
  protected vehicleModel: VehicleDto = {};
  protected manufacturers: ManufacturerDto[] = [];
  protected vehicleTypes: VehicleTypeDto[] = [];
  protected colors: string[] = [
    `AliceBlue`,
    `AntiqueWhite`,
    `Aqua`,
    `Aquamarine`,
    `Azure`,
    `Beige`,
    `Bisque`,
    `Black`,
    `BlanchedAlmond`,
    `Blue`,
    `BlueViolet`,
    `Brown`,
    `BurlyWood`,
    `CadetBlue`,
    `Chartreuse`,
    `Chocolate`,
    `Coral`,
    `CornflowerBlue`,
    `Cornsilk`,
    `Crimson`,
    `Cyan`,
    `DarkBlue`,
    `DarkCyan`,
    `DarkGoldenRod`,
    `DarkGray`,
    `DarkGrey`,
    `DarkGreen`,
    `DarkKhaki`,
    `DarkMagenta`,
    `DarkOliveGreen`,
    `Darkorange`,
    `DarkOrchid`,
    `DarkRed`,
    `DarkSalmon`,
    `DarkSeaGreen`,
    `DarkSlateBlue`,
    `DarkSlateGray`,
    `DarkSlateGrey`,
    `DarkTurquoise`,
    `DarkViolet`,
    `DeepPink`,
    `DeepSkyBlue`,
    `DimGray`,
    `DimGrey`,
    `DodgerBlue`,
    `FireBrick`,
    `FloralWhite`,
    `ForestGreen`,
    `Fuchsia`,
    `Gainsboro`,
    `GhostWhite`,
    `Gold`,
    `GoldenRod`,
    `Gray`,
    `Grey`,
    `Green`,
    `GreenYellow`,
    `HoneyDew`,
    `HotPink`,
    `IndianRed`,
    `Indigo`,
    `Ivory`,
    `Khaki`,
    `Lavender`,
    `LavenderBlush`,
    `LawnGreen`,
    `LemonChiffon`,
    `LightBlue`,
    `LightCoral`,
    `LightCyan`,
    `LightGoldenRodYellow`,
    `LightGray`,
    `LightGrey`,
    `LightGreen`,
    `LightPink`,
    `LightSalmon`,
    `LightSeaGreen`,
    `LightSkyBlue`,
    `LightSlateGray`,
    `LightSlateGrey`,
    `LightSteelBlue`,
    `LightYellow`,
    `Lime`,
    `LimeGreen`,
    `Linen`,
    `Magenta`,
    `Maroon`,
    `MediumAquaMarine`,
    `MediumBlue`,
    `MediumOrchid`,
    `MediumPurple`,
    `MediumSeaGreen`,
    `MediumSlateBlue`,
    `MediumSpringGreen`,
    `MediumTurquoise`,
    `MediumVioletRed`,
    `MidnightBlue`,
    `MintCream`,
    `MistyRose`,
    `Moccasin`,
    `NavajoWhite`,
    `Navy`,
    `OldLace`,
    `Olive`,
    `OliveDrab`,
    `Orange`,
    `OrangeRed`,
    `Orchid`,
    `PaleGoldenRod`,
    `PaleGreen`,
    `PaleTurquoise`,
    `PaleVioletRed`,
    `PapayaWhip`,
    `PeachPuff`,
    `Peru`,
    `Pink`,
    `Plum`,
    `PowderBlue`,
    `Purple`,
    `Red`,
    `RosyBrown`,
    `RoyalBlue`,
    `SaddleBrown`,
    `Salmon`,
    `SandyBrown`,
    `SeaGreen`,
    `SeaShell`,
    `Sienna`,
    `Silver`,
    `SkyBlue`,
    `SlateBlue`,
    `SlateGray`,
    `SlateGrey`,
    `Snow`,
    `SpringGreen`,
    `SteelBlue`,
    `Tan`,
    `Teal`,
    `Thistle`,
    `Tomato`,
    `Turquoise`,
    `Violet`,
    `Wheat`,
    `White`,
    `WhiteSmoke`,
    `Yellow`,
    `YellowGreen`,
  ];

  constructor(
    private formBuilder: FormBuilder,
    private vehicleService: AdminVehicleService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    setTimeout(() => {
      if (this.vehicle) {
        this.mapInputVehicleToModel();
      }
    }, 0);
    this.initializeForm();
    this.getAllManufacturers();
    this.getAllVehicleTypes();
  }


  private mapInputVehicleToModel() {
    this.vehicleModel = {
      id: this.vehicle?.id,
      buildYear: this.vehicle!.buildYear,
      color: this.vehicle!.color,
      manufacturerId: this.vehicle!.manufacturer.id,
      number: this.vehicle!.number,
      registrationNumber: this.vehicle!.registrationNumber,
      vehicleTypeId: this.vehicle!.type.id
    };
  }

  private getAllManufacturers() {
    this.vehicleService
      .getAllManufacturers()
      .pipe(
        tap((response: DataResponse<ManufacturerDto[]>) => {
          this.manufacturers = response.data;
          this.vehicleModel.manufacturerId = this.vehicleModel.manufacturerId ?? response.data[0].id;
        })
      )
      .subscribe();
  }

  private getAllVehicleTypes() {
    this.vehicleService
      .getAllVehicleTypes()
      .pipe(
        tap((response: DataResponse<VehicleTypeDto[]>) => {
          this.vehicleTypes = response.data;
          this.vehicleModel.vehicleTypeId =  this.vehicleModel.vehicleTypeId ?? response.data[0].id;
        })
      )
      .subscribe();
  }

  protected addVehicle() {
    this.vehicleForm.markAllAsTouched();

    if (this.vehicleForm.valid) {
      this.vehicleService.addNewVehicle(this.vehicleModel).subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  private reloadPage() {
    setTimeout(function () {
      window.location.reload();
    }, 1500);
  }

  protected showVehicleTypeModal() {
    this.modalService.clearModalContent();
    this.modalService.adminShowCreateVehicleTypeModal();
  }

  protected showManufacturerModal() {
    this.modalService.clearModalContent();
    this.modalService.adminShowCreateManufacturerModal();
  }

  private initializeForm() {
    this.vehicleForm = this.formBuilder.group(
      {
        vehicleNumber: ['', Validators.required],
        registrationNumber: [
          '',
          Validators.pattern(/[A-z]{1}\d{2}-[A-z]{1}-\d{3}/),
        ],
        buildYear: ['', Validators.required],
      },
      { updateOn: 'change' }
    );
  }

  protected editVehicle() {
    this.vehicleService.editVehicle(this.vehicleModel)
      .subscribe(this.reloadPage);
  }
}
