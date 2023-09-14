import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { UserService } from '../services/user/user.service';
import { CacheService } from '../services/cache/cache.service';
import { ToastMessageService } from '../services/toast/toast-message.service';
import { LocalizationService } from '../services/localization/localization.service';

@Injectable({
  providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {

  constructor(
    private userService: UserService,
    private router: Router,
    private cacheService: CacheService,
    private toastService: ToastMessageService,
    private localizationService: LocalizationService) {}

  canActivate(activatedRoute: ActivatedRouteSnapshot) {
    if (this.userService.isLoggedIn()) {
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
