import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserRequestModalComponent } from './admin-user-request-modal.component';

describe('AdminUserRequestModalComponent', () => {
  let component: AdminUserRequestModalComponent;
  let fixture: ComponentFixture<AdminUserRequestModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminUserRequestModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminUserRequestModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
