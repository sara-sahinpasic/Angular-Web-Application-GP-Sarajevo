import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-activate-account',
  templateUrl: './activate-account.component.html',
  styleUrls: ['./activate-account.component.scss']
})
export class ActivateAccountComponent implements OnInit {

  private token: string = "";
  message: string = "";

  constructor(private activatedRoute: ActivatedRoute, private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.token = this.activatedRoute.snapshot.paramMap.get("token") as string;

    const observer: Observable<string> = this.userService.activateAccount(this.token);
    observer.subscribe(() => {
      this.message = "Račun aktiviran!";
      setInterval(()=> window.location.replace("/**"),2500);
    });
  }
}


