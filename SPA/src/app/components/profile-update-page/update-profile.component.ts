import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { UserEditPrfileModel } from 'src/app/models/User/UserEditProfileModel';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.scss'],
})
export class UpdateProfileComponent implements OnInit {

  protected formGroup!: FormGroup;

  constructor(private _userService: UserService, protected localizationService: LocalizationService, private formBuilder: FormBuilder) {}

  profileModel: UserEditPrfileModel = {};

  ngOnInit(): void {
    this._userService.user$
      .pipe(
        tap(this.mapResultToModel.bind(this))
      )
      .subscribe();

      this.initializeValidators();
  }

  initializeValidators() {
    this.formGroup = this.formBuilder.group({
      password: ["", Validators.minLength(8)]
    })
  }

  mapResultToModel(user: UserProfileModel | undefined) {
    this.profileModel.address = user?.address;
    this.profileModel.firstName = user?.firstName;
    this.profileModel.lastName = user?.lastName;
    this.profileModel.id = user?.id;
    this.profileModel.phoneNumber = user?.phoneNumber;
    this.profileModel.profileImageBase64 = user?.profileImageBase64;
  }

  save() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    this.profileModel.password = null;
console.log(this.formGroup)
    if (this.formGroup.get("password")?.value != "") {
      this.profileModel.password = this.formGroup.get("password")?.value;
    }

    this._userService.updateUser(this.profileModel, '/profile').subscribe();
  }

  uploadImage(event: Event, profileImageElement: HTMLImageElement) {
    const inputEvent: InputEvent = event as InputEvent;
    const inputElement: HTMLInputElement =
      inputEvent.target as HTMLInputElement;
    const fileReader: FileReader = new FileReader();
    let base64String: string;
    this.profileModel.profileImageFile = inputElement.files?.item(0) as File;
    fileReader.readAsDataURL(inputElement.files?.item(0) as Blob);
    fileReader.onload = () => {
      base64String = fileReader.result as string;
      profileImageElement.src = base64String;
    };
  }
}
