import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminHolidayModalComponent } from './admin-holiday-modal.component';

describe('AdminHolidayModalComponent', () => {
  let component: AdminHolidayModalComponent;
  let fixture: ComponentFixture<AdminHolidayModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminHolidayModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminHolidayModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
