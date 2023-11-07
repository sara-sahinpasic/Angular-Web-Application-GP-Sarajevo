import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminHolidaysPageComponent } from './admin-holidays-page.component';

describe('AdminHolidaysPageComponent', () => {
  let component: AdminHolidaysPageComponent;
  let fixture: ComponentFixture<AdminHolidaysPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminHolidaysPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminHolidaysPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
