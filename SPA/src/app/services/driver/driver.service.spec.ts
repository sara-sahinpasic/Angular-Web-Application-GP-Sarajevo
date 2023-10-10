import { TestBed } from '@angular/core/testing';

import { DriverDelayService } from './driver.service';

describe('DriverDelayService', () => {
  let service: DriverDelayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DriverDelayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
