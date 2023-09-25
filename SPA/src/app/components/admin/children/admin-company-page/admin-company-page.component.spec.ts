import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCompanyPageComponent } from './admin-company-page.component';

describe('AdminCompanyPageComponent', () => {
  let component: AdminCompanyPageComponent;
  let fixture: ComponentFixture<AdminCompanyPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCompanyPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminCompanyPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
