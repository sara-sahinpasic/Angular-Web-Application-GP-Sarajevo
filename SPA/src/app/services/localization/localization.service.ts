import { Injectable } from '@angular/core';
import { InvalidArgumentException } from 'src/app/exceptions/InvalidArgumentException';
import { Messages } from 'src/localization/messages';
import { CacheService } from '../cache/cache.service';

@Injectable({
  providedIn: 'root'
})
export class LocalizationService {

  private localizationIdentifiers: string[] = ["en", "bs"]
  private locale!: string | null;

  constructor(private cacheService: CacheService) {
    this.locale = this.getLocale();
  }

  public localize(message: string): string {
    let localeLower = "bs";

    if (this.locale != null) {
      localeLower = this.locale.toLowerCase();
    }

    if (this.localizationIdentifiers.indexOf(localeLower) == -1) {
      localeLower = "bs";
    }

    let localizedMessage = Messages.locale[localeLower][message];

    if (localizedMessage == undefined) {
      localizedMessage = message;
    }

    return localizedMessage;
  }

  public setLocale(locale: string) {
    const localeLower = locale.toLowerCase();

    if (this.localizationIdentifiers.indexOf(localeLower) == -1) {
      throw new InvalidArgumentException(["locale must be either en or bs"]);
    }

    this.cacheService.setCacheData("locale", locale);
  }

  public getLocale(): string | null {
    return this.cacheService.getDataFromCache("locale");
  }
}
