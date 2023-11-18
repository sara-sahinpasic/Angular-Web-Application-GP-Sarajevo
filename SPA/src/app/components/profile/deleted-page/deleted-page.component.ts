import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-profile-deleted-page',
  templateUrl: './deleted-page.component.html',
  styleUrls: ['./deleted-page.component.scss']
})
export class ProfileDeletedPageComponent implements OnInit {

  constructor(
    protected localizationService: LocalizationService,
    private userService: UserService,
    private router: Router) {}

  ngOnInit() {
    this.userService.isDeleteRequestSent$.pipe(
        tap((value: boolean) => {
          if (!value) {
            this.router.navigateByUrl('');
          }
        })
      )
      .subscribe();
  }
}
