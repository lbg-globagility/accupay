import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeLogFormComponent } from './time-log-form.component';

describe('TimeLogFormComponent', () => {
  let component: TimeLogFormComponent;
  let fixture: ComponentFixture<TimeLogFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeLogFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeLogFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
