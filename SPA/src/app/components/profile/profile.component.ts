import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  public profileModel!: UserProfileModel;
  showModalRequest: boolean = false;
  showModalPurchaseHistory: boolean = false;

  constructor(private _router: Router, private userService: UserService) {}

  ngOnInit(): void {
    this.userService.user$
      .pipe(
        tap((user: UserProfileModel | undefined) => (this.profileModel = user!))
      )
      .subscribe();
  }

  navigateToProfile() {
    this._router.navigateByUrl('/profile');
  }

  navigateToUpdate() {
    const id: string = this.profileModel?.id as string;

    this._router.navigateByUrl(`/update`);
  }

  deleteProfile() {
    const id: string = this.profileModel?.id as string;
    const answer: boolean = confirm('Da li želite obrisati račun?');

    if (answer == true) {
      this.userService.deleteUser(id).subscribe();
    }
  }

  navigateToPurchaseHistory() {
    this.showModalPurchaseHistory = true;
  }
  navigateToRequest() {
    this.showModalRequest = true;
  }
  requestCloseButton() {
    this.showModalRequest = false;
  }
  historyCloseButton(){
      this.showModalPurchaseHistory=false;
  }
}
