import { TestBed } from '@angular/core/testing';

import { CheckoutConfirmGuard } from './checkout-confirm.guard';

describe('CheckoutConfirmGuard', () => {
  let guard: CheckoutConfirmGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(CheckoutConfirmGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
