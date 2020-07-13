import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ThirteenthMonthReportComponent } from './thirteenth-month-report.component';

describe('ThirteenthMonthReportComponent', () => {
  let component: ThirteenthMonthReportComponent;
  let fixture: ComponentFixture<ThirteenthMonthReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ThirteenthMonthReportComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThirteenthMonthReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
