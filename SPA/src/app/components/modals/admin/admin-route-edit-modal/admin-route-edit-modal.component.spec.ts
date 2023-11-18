import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRouteEditModalComponent } from './admin-route-edit-modal.component';

describe('AdminRouteEditModalComponent', () => {
  let component: AdminRouteEditModalComponent;
  let fixture: ComponentFixture<AdminRouteEditModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminRouteEditModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRouteEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
