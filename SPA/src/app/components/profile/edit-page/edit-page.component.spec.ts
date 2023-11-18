import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProfilePage } from './edit-page.component';

describe('UpdateProfileComponent', () => {
  let component: EditProfilePage;
  let fixture: ComponentFixture<EditProfilePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProfilePage ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditProfilePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
