import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.css']
})
export class LogInComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }
  prijava(){
    this.router.navigateByUrl("");

  }
  registracija(){
this.router.navigateByUrl("registracija");
  }
}
