import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeEntryDetailsComponent } from './time-entry-details.component';

describe('TimeEntryComponent', () => {
  let component: TimeEntryDetailsComponent;
  let fixture: ComponentFixture<TimeEntryDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TimeEntryDetailsComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeEntryDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
