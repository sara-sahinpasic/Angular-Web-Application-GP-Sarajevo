import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminNavPageComponent } from './admin-nav-page.component';

describe('AdminNavPageComponent', () => {
  let component: AdminNavPageComponent;
  let fixture: ComponentFixture<AdminNavPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminNavPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminNavPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
