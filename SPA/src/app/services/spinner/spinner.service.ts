import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SpinnerService {
  private loading: boolean = false;

  constructor() {}

  setLoading(loading: boolean) {
    this.loading = loading;
  }
  getLoading(): boolean {
    return this.loading;
  }
}
