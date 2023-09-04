import { TestBed } from '@angular/core/testing';

import { RouteNotSetGuard } from './route-not-set.guard';

describe('RouteNotSetGuard', () => {
  let guard: RouteNotSetGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(RouteNotSetGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
