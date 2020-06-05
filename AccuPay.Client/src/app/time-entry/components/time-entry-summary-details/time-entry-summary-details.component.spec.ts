import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeEntrySummaryDetailsComponent } from './time-entry-summary-details.component';

describe('TimeEntrySummaryDetailsComponent', () => {
  let component: TimeEntrySummaryDetailsComponent;
  let fixture: ComponentFixture<TimeEntrySummaryDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TimeEntrySummaryDetailsComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeEntrySummaryDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
