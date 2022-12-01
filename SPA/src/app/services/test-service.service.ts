import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TestServiceService {

constructor() { }

  testMethod() {
    alert("testiraj me");
  }
}
