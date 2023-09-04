import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CacheService {

  private expiryTime: number = 1000 * 60 * 30;

  constructor() { }

  public getDataFromCache<T>(cacheKey: string): T | null {
    const cache: string | null = localStorage.getItem(cacheKey);

    if (!cache) {
      return null;
    }

    const cacheData: CacheWithExpiry<T> = JSON.parse(cache);
    const currentTimeInMillis: number = new Date().getTime();
    const milliSecondsDifference: number = currentTimeInMillis - cacheData.expiryTimeInMillis;

    if (milliSecondsDifference > this.expiryTime) {
      this.unsetCacheItem(cacheKey);
      return null;
    }

    return cacheData.data;
  }

  public setCacheData(cacheKey: string, data: any) {
    const cacheData: CacheWithExpiry<any> = {
      expiryTimeInMillis: new Date().getTime(),
      data
    };

    localStorage.setItem(cacheKey, JSON.stringify(cacheData));
  }

  public unsetCacheItem(cacheKey: string) {
    localStorage.removeItem(cacheKey);
  }
}

interface CacheWithExpiry<T> {
  expiryTimeInMillis: number,
  data: T
}
