import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Profile } from 'src/app/models/User/Profile';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.scss'],
})
export class UpdateProfileComponent implements OnInit {
  constructor(
    private _httpClient: HttpClient,
    private _route: ActivatedRoute,
    private _router: Router
  ) {}
  private url: string = environment.apiUrl;

  ngOnInit(): void {
    const id: string = this._route.snapshot.paramMap.get('id') as string;
    this._httpClient.get(`${this.url}Profile?id=${id}`).subscribe((p: any) => {
      this.profileModel = p;
    });
  }

  profileModel: Profile = {
    id: '',
    firstName: '',
    lastName: '',
    dateOfBirth: new Date(),
    phoneNumber: '',
    address: '',
    email: '',
  };

  save() {
    const id: string = this._route.snapshot.paramMap.get('id') as string;
    this._httpClient
      .put(`${this.url}Profile`, this.profileModel)
      .subscribe();
  }
  ucitajFotografiju() {
    throw new Error('Method not implemented.');
  }
}
