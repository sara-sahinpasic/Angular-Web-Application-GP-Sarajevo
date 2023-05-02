import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Profile } from 'src/app/models/User/Profile';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

constructor(private _httpClient:HttpClient, private _route: ActivatedRoute, private _router: Router){

}
adresa_servera = "http://localhost:7192";
ngOnInit(): void{
  const id: number = Number(this._route.snapshot.paramMap.get("id"));
  this._httpClient.get(`${this.adresa_servera}/Profile?id=${id}`)
    .subscribe((p: any) => {
      this.profileModel = p;
    });
}

profileModel: Profile = {
  id:'',
  ime:'',
  prezime:'',
  datumRodjenja:'',
  brojTelefona:'',
  adresa:'',
  email:'',
}

historijaKupovina() {
throw new Error('Method not implemented.');
}
mojiPodaci() {
  //this._router.navigateByUrl("/update");
}
izmjenaPodataka() {
  this._router.navigateByUrl("/update");
}

}


