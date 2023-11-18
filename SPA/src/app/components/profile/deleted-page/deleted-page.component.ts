import { Component } from '@angular/core';
import { LocalizationService } from 'src/app/services/localization/localization.service';

@Component({
  selector: 'app-profile-deleted-page',
  templateUrl: './deleted-page.component.html',
  styleUrls: ['./deleted-page.component.scss']
})
export class ProfileDeletedPageComponent {
  constructor(protected localizationService: LocalizationService) {}
}
