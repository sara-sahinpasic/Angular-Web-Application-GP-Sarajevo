import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVehicleMalfunctionDetailsComponent } from './admin-vehicle-malfunction-details.component';

describe('AdminVehicleMalfunctionDetailsComponent', () => {
  let component: AdminVehicleMalfunctionDetailsComponent;
  let fixture: ComponentFixture<AdminVehicleMalfunctionDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminVehicleMalfunctionDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminVehicleMalfunctionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
