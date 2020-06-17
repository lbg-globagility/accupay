import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeLogs2Component } from './time-logs2.component';

describe('TimeLogs2Component', () => {
  let component: TimeLogs2Component;
  let fixture: ComponentFixture<TimeLogs2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeLogs2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeLogs2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
