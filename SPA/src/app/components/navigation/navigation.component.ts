import { Component, OnInit } from '@angular/core';
import { faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {

  protected locale!: string | null;
  protected user?: UserProfileModel;
  protected logoutIcon = faRightFromBracket;

  constructor(private userService: UserService, protected localizationService: LocalizationService) { }

  ngOnInit() {
    this.locale = this.localizationService.getLocale();

    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => {
        this.user = user;
      })
    )
    .subscribe();
  }

  logout() {
    this.userService.logout();
  }

  setLocale(locale: string) {
    this.localizationService.setLocale(locale);
    window.location.reload();
  }
}
