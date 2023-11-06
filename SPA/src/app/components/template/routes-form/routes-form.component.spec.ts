import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoutesFormComponent } from './routes-form.component';

describe('RoutesFormComponent', () => {
  let component: RoutesFormComponent;
  let fixture: ComponentFixture<RoutesFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoutesFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoutesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
