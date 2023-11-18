import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/user/userProfileModel';
import { UserProfileStatusModel } from 'src/app/models/status/userProfileStatusModel';
import { CacheService } from 'src/app/services/cache/cache.service';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { UserStatusService } from 'src/app/services/user/user-status.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  protected profileModel: UserProfileModel = {} as UserProfileModel;
  protected showModalRequest: boolean = false;
  protected locale: string | null = null;
  protected userStatusModel: UserProfileStatusModel = {} as UserProfileStatusModel;

  constructor(
    private router: Router,
    private userService: UserService,
    private modalService: ModalService,
    private statusService: UserStatusService,
    protected localizationService: LocalizationService,
    private cacheService: CacheService
  ) {}

  ngOnInit() {
    this.setUser();
    this.setProfileImage();
    this.locale = this.localizationService.getLocale();
  }

  private setProfileImage() {
    this.userService.getProfileImage().subscribe(
      {
        next: (x) => (this.profileModel.profileImageBase64 = x.data),
        error: () => (this.profileModel.profileImageBase64 = './assets/X.png'),
      });
  }

  private setUser() {
    this.userService.user$
      .pipe(
        tap(this.mapUser.bind(this))
      )
      .subscribe();
  }

  private mapUser(user: UserProfileModel | undefined) {
    this.profileModel = user!;

    const userStatusModelFromCache: UserProfileStatusModel | null = this.cacheService.getDataFromCache('profile_userStatus');
    if (userStatusModelFromCache) {
      this.userStatusModel = userStatusModelFromCache;
      return;
    }

    this.statusService.getUserStatus(user!.id!)
      .pipe(
        tap((response: UserProfileStatusModel) => {
          this.userStatusModel = response;
          this.cacheService.setCacheData('profile_userStatus', response);
        })
      )
      .subscribe();
  }

  protected navigateToUpdate() {
    this.router.navigateByUrl('/update');
  }

  protected deleteProfile() {
    const id: string = this.profileModel?.id as string;
    const answer: boolean = confirm(this.localizationService.localize('edit_profile_deletion_confirmation_text'));

    if (answer == true) {
      this.userService.deleteUser(id).subscribe();
    }
  }

  protected navigateToPurchaseHistory() {
    this.router.navigateByUrl('/purchaseHistory');
  }

  protected showRequestModal() {
    this.modalService.setModalTitle(this.localizationService.localize('request_modal_title'));
    this.modalService.showRequestModal();
  }
}
