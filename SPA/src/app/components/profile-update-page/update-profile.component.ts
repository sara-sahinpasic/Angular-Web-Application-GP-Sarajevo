import { Component, OnInit } from '@angular/core';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.scss'],
})
export class UpdateProfileComponent implements OnInit {
  constructor(private _userService: UserService) {}

  profileModel: UserProfileModel = {
    id: '',
    firstName: '',
    lastName: '',
    dateOfBirth: new Date(),
    phoneNumber: '',
    address: '',
    email: '',
  };

  ngOnInit(): void {
    this._userService.user$
      .pipe(
        tap((user: UserProfileModel | undefined) => (this.profileModel = user!))
      )
      .subscribe();
  }

  save() {
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
