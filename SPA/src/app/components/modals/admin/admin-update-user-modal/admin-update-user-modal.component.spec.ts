import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUpdateUserModalComponent } from './admin-update-user-modal.component';

describe('AdminUpdateUserModalComponent', () => {
  let component: AdminUpdateUserModalComponent;
  let fixture: ComponentFixture<AdminUpdateUserModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminUpdateUserModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminUpdateUserModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
