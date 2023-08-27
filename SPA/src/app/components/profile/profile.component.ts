import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  public profileModel!: UserProfileModel;
  protected showModalRequest: boolean = false;
  protected locale!: string | null;

  constructor(
    private _router: Router,
    private userService: UserService,
    private modalService: ModalService,
    protected localizationService: LocalizationService
  ) {}

  ngOnInit(): void {
    this.userService.user$
      .pipe(
        tap((user: UserProfileModel | undefined) => (this.profileModel = user!))
      )
      .subscribe();
    this.userService.getProfileImage().subscribe(
      {
      next: (x) => (this.profileModel.profileImageBase64 = x.data),
      error: () => (this.profileModel.profileImageBase64 = "./assets/X.png"),
    });

    this.locale = this.localizationService.getLocale();
  }

  navigateToProfile() {
    this._router.navigateByUrl('/profile');
  }

  navigateToUpdate() {
    const id: string = this.profileModel?.id as string;

    this._router.navigateByUrl(`/update`);
  }

  deleteProfile() {
    const id: string = this.profileModel?.id as string;
    const answer: boolean = confirm('Da li želite obrisati račun?');

    if (answer == true) {
      this.userService.deleteUser(id).subscribe();
    }
  }

  navigateToPurchaseHistory() {
    this._router.navigateByUrl(`/purchaseHistory`);
  }

  showModal() {
    this.modalService.setModalTitle(this.localizationService.localize("request_modal_title"));
    this.modalService.showRequestModal();
  }
}
