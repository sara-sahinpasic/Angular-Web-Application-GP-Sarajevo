import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketModalComponent } from './admin-ticket-modal.component';

describe('AdminTicketModalComponent', () => {
  let component: AdminTicketModalComponent;
  let fixture: ComponentFixture<AdminTicketModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminTicketModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
