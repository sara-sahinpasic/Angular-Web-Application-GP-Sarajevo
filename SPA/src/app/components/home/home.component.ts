import { Component } from '@angular/core';
import { TestServiceService } from 'src/app/services/test-service.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(private testService: TestServiceService) { }

  click() {
    this.testService.testMethod();
  }
}
