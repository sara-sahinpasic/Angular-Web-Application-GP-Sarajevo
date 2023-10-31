import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVehiclePageComponent } from './admin-vehicle-page.component';

describe('AdminVehiclePageComponent', () => {
  let component: AdminVehiclePageComponent;
  let fixture: ComponentFixture<AdminVehiclePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminVehiclePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminVehiclePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
