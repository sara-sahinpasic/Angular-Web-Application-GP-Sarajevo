import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRoutesPageComponent } from './admin-routes-page.component';

describe('AdminRoutesPageComponent', () => {
  let component: AdminRoutesPageComponent;
  let fixture: ComponentFixture<AdminRoutesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminRoutesPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRoutesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
