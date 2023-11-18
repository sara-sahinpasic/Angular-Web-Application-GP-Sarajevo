import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { UserEditProfileModel } from 'src/app/models/user/userEditProfileModel';
import { UserProfileModel } from 'src/app/models/user/userProfileModel';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-edit-profile-page',
  templateUrl: './edit-page.component.html',
  styleUrls: ['./edit-page.component.scss'],
})
export class EditProfilePage implements OnInit {
  protected profileModel: UserEditProfileModel = {};
  protected formGroup: FormGroup = {} as FormGroup;

  constructor(
    private userService: UserService,
    protected localizationService: LocalizationService,
    private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.fillUserData();
    this.setProfileImage();
  }

  private setProfileImage() {
    this.userService.getProfileImage().subscribe(
      {
        next: (x) => (this.profileModel.profileImageBase64 = x.data),
        error: () => (this.profileModel.profileImageBase64 = './assets/X.png'),
      });
  }

  private fillUserData() {
    this.userService.user$
      .pipe(
        tap(this.initializeForm.bind(this))
      )
      .subscribe();
  }

  private initializeForm(user: UserProfileModel | undefined) {
    this.formGroup = this.formBuilder.group({
      id: [user?.id, null],
      firstName: [user?.firstName, Validators.required],
      lastName: [user?.lastName, Validators.required],
      address: [user?.address, null],
      phoneNumber: [user?.phoneNumber, Validators.pattern('[- +()0-9]+')],
      password: ['', Validators.minLength(8)]
    });
  }

  protected save() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    this.profileModel.password = null;

    Object.keys(this.formGroup.controls).forEach((key: string) => {
      this.profileModel[key] = this.formGroup.value[key];

      if (key === 'password' && this.formGroup.value[key] != '') {
        this.profileModel.password = this.formGroup.value[key];
      }
    });

    this.userService.updateUser(this.profileModel, '/profile').subscribe();
  }

  protected uploadImage(event: Event, profileImageElement: HTMLImageElement) {
    const inputEvent: InputEvent = event as InputEvent;
    const inputElement: HTMLInputElement = inputEvent.target as HTMLInputElement;
    const fileReader: FileReader = new FileReader();

    this.profileModel.profileImageFile = inputElement.files?.item(0) as File;

    fileReader.readAsDataURL(inputElement.files?.item(0) as Blob);
    fileReader.onload = () => {
      profileImageElement.src = fileReader.result as string;
    };
  }
}
