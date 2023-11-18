import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVehicleTypeModalComponent } from './admin-vehicle-type-modal.component';

describe('AdminVehicleTypeModalComponent', () => {
  let component: AdminVehicleTypeModalComponent;
  let fixture: ComponentFixture<AdminVehicleTypeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminVehicleTypeModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminVehicleTypeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
