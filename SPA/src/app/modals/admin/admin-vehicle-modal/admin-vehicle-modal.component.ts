import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { ManufacturerDto } from 'src/app/models/Admin/Vehicle/ManufacturerDto';
import { VehicleDto } from 'src/app/models/Admin/Vehicle/VehicleDto';
import { VehicleTypeDto } from 'src/app/models/Admin/Vehicle/VehicleTypeDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { AdminVehicleService } from 'src/app/services/admin/vehicle/admin-vehicle.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-vehicle-modal',
  templateUrl: './admin-vehicle-modal.component.html',
  styleUrls: ['./admin-vehicle-modal.component.scss'],
})
export class AdminVehicleModalComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    protected vehicleService: AdminVehicleService,
    protected router: Router,
    protected modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.getAllManufacturers();
    this.getAllVehicleTypes();
  }

  registrationForm!: FormGroup;
  protected vehicleModel: VehicleDto = {};
  protected manufacturers: Array<ManufacturerDto> = [];
  protected vehicleTypes: Array<VehicleTypeDto> = [];

  getAllManufacturers() {
    this.vehicleService
      .getAllManufacturers()
      .pipe(
        tap((response: DataResponse<ManufacturerDto[]>) => {
          this.manufacturers = response.data;
          this.vehicleModel.manufacturerId = response.data[0].id;
        })
      )
      .subscribe();
  }

  getAllVehicleTypes() {
    this.vehicleService
      .getAllVehicleTypes()
      .pipe(
        tap((response: DataResponse<VehicleTypeDto[]>) => {
          this.vehicleTypes = response.data;
          this.vehicleModel.vehicleTypeId = response.data[0].id;
        })
      )
      .subscribe();
  }

  addVehicle() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.vehicleService.addNewVehicle(this.vehicleModel).subscribe(() => {
        setTimeout(function () {
          window.location.reload();
        }, 3000);
      });
    }
  }

  showVehicleTypeModal() {
    this.modalService.clearModalContent();
    this.modalService.adminShowCreateVehicleTypeModal();
  }

  showManufacturerModal() {
    this.modalService.clearModalContent();
    this.modalService.adminShowCreateManufacturerModal();
  }

  initForm() {
    this.registrationForm = this.formBuilder.group(
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

  protected colors: Array<string> = [
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
}
