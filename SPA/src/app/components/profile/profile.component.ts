import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  private url: string = environment.apiUrl;
  public profileModel!: UserProfileModel;

  constructor(
    private _httpClient: HttpClient,
    private _route: ActivatedRoute,
    private _router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.profileModel = this.userService.getUser() as UserProfileModel;
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
      this._httpClient.delete(`${this.url}Profile?id=${id}`).subscribe(() => {
        this.userService.logout();
      });
      this._router.navigateByUrl('/delete');
    } else {
      this._router.navigateByUrl('/profile');
    }
  }

  navigateToPurchaseHistory() {
    throw new Error('Method not implemented.');
  }
}
