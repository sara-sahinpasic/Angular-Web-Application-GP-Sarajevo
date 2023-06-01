import { Component, OnInit } from '@angular/core';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.scss'],
})
export class UpdateProfileComponent implements OnInit {
  constructor(private _userService: UserService) { }

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
    this._userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => this.profileModel = user!)
    )
    .subscribe();
  }

  save() {
    this._userService.updateUser(this.profileModel, '/profile').subscribe();
  }

  // todo
  ucitajFotografiju() {
    throw new Error('Method not implemented.');
  }
}
