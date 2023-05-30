import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-activate-account',
  templateUrl: './activate-account.component.html',
  styleUrls: ['./activate-account.component.scss']
})
export class ActivateAccountComponent implements OnInit {

  private token: string = "";
  isActivated: boolean = false;
  // todo: create a centralized error handler for API errors
  constructor(private activatedRoute: ActivatedRoute, private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.userService.isActivated$.pipe(
      tap((val: boolean) => this.isActivated = val)
    )
      .subscribe();

    this.token = this.activatedRoute.snapshot.paramMap.get("token") as string;

    this.userService.activateAccount(this.token).subscribe();
  }
}


