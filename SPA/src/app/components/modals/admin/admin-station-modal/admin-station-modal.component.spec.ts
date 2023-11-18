import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminStationModalComponent } from './admin-station-modal.component';

describe('AdminStationModalComponent', () => {
  let component: AdminStationModalComponent;
  let fixture: ComponentFixture<AdminStationModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminStationModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminStationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
