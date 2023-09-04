import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVehicleModalComponent } from './admin-vehicle-modal.component';

describe('AdminVehicleModalComponent', () => {
  let component: AdminVehicleModalComponent;
  let fixture: ComponentFixture<AdminVehicleModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminVehicleModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminVehicleModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
