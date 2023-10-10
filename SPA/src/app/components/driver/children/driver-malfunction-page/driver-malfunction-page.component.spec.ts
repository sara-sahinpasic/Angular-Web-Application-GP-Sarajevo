import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverMalfunctionPageComponent } from './driver-malfunction-page.component';

describe('DriverMalfunctionPageComponent', () => {
  let component: DriverMalfunctionPageComponent;
  let fixture: ComponentFixture<DriverMalfunctionPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DriverMalfunctionPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DriverMalfunctionPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
