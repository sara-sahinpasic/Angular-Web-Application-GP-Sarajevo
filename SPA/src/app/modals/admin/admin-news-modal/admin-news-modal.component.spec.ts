import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminNewsModalComponent } from './admin-news-modal.component';

describe('AdminNewsModalComponent', () => {
  let component: AdminNewsModalComponent;
  let fixture: ComponentFixture<AdminNewsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminNewsModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminNewsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
