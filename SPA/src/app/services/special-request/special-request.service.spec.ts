import { TestBed } from '@angular/core/testing';

import { SpecialRequestService } from './special-request.service';

describe('FileUploadService', () => {
  let service: SpecialRequestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SpecialRequestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
