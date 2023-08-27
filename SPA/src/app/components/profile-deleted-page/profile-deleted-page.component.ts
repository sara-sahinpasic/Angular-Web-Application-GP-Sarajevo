import { Component } from '@angular/core';
import { LocalizationService } from 'src/app/services/localization/localization.service';

@Component({
  selector: 'app-profile-deleted-page',
  templateUrl: './profile-deleted-page.component.html',
  styleUrls: ['./profile-deleted-page.component.scss']
})
export class ProfileDeletedPageComponent {
  constructor(protected localizationService: LocalizationService) {}
}
