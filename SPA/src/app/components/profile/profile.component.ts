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
    // const id: string = this._route.snapshot.paramMap.get('id') as string;
    // this._httpClient.get(`${this.url}Profile?id=${id}`).subscribe((p: any) => {
    //   this.profileModel = p;
    // });
    this.profileModel = this.userService.getUser() as UserProfileModel;
    console.log(this.profileModel);
  }


  navigateToProfile() {
    this._router.navigateByUrl('/profile/:id');
  }
  navigateToUpdate() {
    this._router.navigateByUrl('/update/:id');
  }
  deleteProfile() {
    const id: string = this.profileModel?.id as string;

    this._httpClient.delete(`${this.url}Profile?id=${id}`).subscribe(() => {
      this._router.navigateByUrl('/delete/:id');
    });
    //setInterval(()=> this._router.navigateByUrl("/**"),2500);
  }
  navigateToPurchaseHistory() {
    throw new Error('Method not implemented.');
  }
}
