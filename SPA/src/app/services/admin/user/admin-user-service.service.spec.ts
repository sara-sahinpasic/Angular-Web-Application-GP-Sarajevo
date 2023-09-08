import { TestBed } from '@angular/core/testing';

import { AdminUserService } from './admin-User-Service';

describe('AdminUserCreateService', () => {
  let service: AdminUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
