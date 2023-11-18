import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-activate-account',
  templateUrl: './activate-account.component.html',
  styleUrls: ['./activate-account.component.scss']
})
export class ActivateAccountComponent implements OnInit {
  constructor(private activatedRoute: ActivatedRoute, private userService: UserService,
    private router: Router) { }

  ngOnInit(): void {
    const token: string | undefined = this.activatedRoute.snapshot.paramMap.get("token") as string;

    if (!token) {
      this.router.navigateByUrl('');
      return;
    }

    this.userService.activateAccount(token)
      .subscribe(() => this.router.navigateByUrl('login'));
  }
}


