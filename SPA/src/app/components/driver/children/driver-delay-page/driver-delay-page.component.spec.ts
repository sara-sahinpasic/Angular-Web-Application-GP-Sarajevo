import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverDelayPageComponent } from './driver-delay-page.component';

describe('DriverDelayPageComponent', () => {
  let component: DriverDelayPageComponent;
  let fixture: ComponentFixture<DriverDelayPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DriverDelayPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DriverDelayPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
