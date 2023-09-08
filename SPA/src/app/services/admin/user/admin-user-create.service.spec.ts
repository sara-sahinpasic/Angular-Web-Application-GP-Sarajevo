import { TestBed } from '@angular/core/testing';

import { AdminUserCreateService } from './admin-user-create.service';

describe('AdminUserCreateService', () => {
  let service: AdminUserCreateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminUserCreateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
