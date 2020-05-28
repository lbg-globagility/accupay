import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewShiftComponent } from './new-shift.component';

describe('NewShiftComponent', () => {
  let component: NewShiftComponent;
  let fixture: ComponentFixture<NewShiftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewShiftComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewShiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
