import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminNewStationModalComponent } from './admin-new-station-modal.component';

describe('AdminNewStationModalComponent', () => {
  let component: AdminNewStationModalComponent;
  let fixture: ComponentFixture<AdminNewStationModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminNewStationModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminNewStationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
