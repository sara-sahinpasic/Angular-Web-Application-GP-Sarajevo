import { TestBed } from '@angular/core/testing';

import { AdminTicketService } from './admin-ticket.service';

describe('AdminTicketService', () => {
  let service: AdminTicketService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminTicketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
