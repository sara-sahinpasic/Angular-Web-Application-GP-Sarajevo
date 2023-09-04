import { Injectable, OnInit } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { UserProfileModel } from '../models/User/UserProfileModel';
import { tap } from 'rxjs';
import { UserService } from '../services/user/user.service';
import { CacheService } from '../services/cache/cache.service';
import { ToastMessageService } from '../services/toast/toast-message.service';
import { LocalizationService } from '../services/localization/localization.service';

@Injectable({
  providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {

  isLoggedIn: any;

  constructor(
    private userService: UserService,
    private router: Router,
    private cacheService: CacheService,
    private toastService: ToastMessageService,
    private localizationService: LocalizationService) {
    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => this.isLoggedIn = user))
    .subscribe();
  }

  canActivate(activatedRoute: ActivatedRouteSnapshot) {
    if (this.isLoggedIn != undefined) {
      return true;
    }
    else {
      this.router.navigateByUrl("/login");
      this.cacheService.setCacheData("redirectUrl", activatedRoute.url[0].path);
      this.toastService.pushErrorMessage(this.localizationService.localize("must_login_error_message"))
      return false;
    }
  }
}
