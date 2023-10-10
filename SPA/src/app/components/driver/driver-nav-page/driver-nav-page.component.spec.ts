import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverNavPageComponent } from './driver-nav-page.component';

describe('DriverNavPageComponent', () => {
  let component: DriverNavPageComponent;
  let fixture: ComponentFixture<DriverNavPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DriverNavPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DriverNavPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
