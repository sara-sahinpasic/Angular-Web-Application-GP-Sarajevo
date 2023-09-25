import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManufacturerModalComponent } from './admin-manufacturer-modal.component';

describe('AdminManufacturerModalComponent', () => {
  let component: AdminManufacturerModalComponent;
  let fixture: ComponentFixture<AdminManufacturerModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminManufacturerModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminManufacturerModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
