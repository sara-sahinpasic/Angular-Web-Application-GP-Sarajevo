import { Component, OnInit } from '@angular/core';
import { faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {

  protected user?: UserProfileModel;
  protected logoutIcon = faRightFromBracket;
  constructor(private userService: UserService) { }

  ngOnInit() {
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
}
