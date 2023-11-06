import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRouteModalComponent } from './admin-route-modal.component';

describe('AdminRouteModalComponent', () => {
  let component: AdminRouteModalComponent;
  let fixture: ComponentFixture<AdminRouteModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminRouteModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRouteModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
