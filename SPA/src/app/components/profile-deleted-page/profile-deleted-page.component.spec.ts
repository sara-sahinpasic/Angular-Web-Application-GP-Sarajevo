import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileDeletedPageComponent } from './profile-deleted-page.component';

describe('ProfileDeletedPageComponent', () => {
  let component: ProfileDeletedPageComponent;
  let fixture: ComponentFixture<ProfileDeletedPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileDeletedPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileDeletedPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
