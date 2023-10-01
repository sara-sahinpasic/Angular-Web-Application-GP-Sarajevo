import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketsPageComponent } from './admin-tickets-page.component';

describe('AdminTicketsPageComponent', () => {
  let component: AdminTicketsPageComponent;
  let fixture: ComponentFixture<AdminTicketsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminTicketsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
