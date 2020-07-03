import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ThirtheenthMonthReportComponent } from './thirtheenth-month-report.component';

describe('ThirtheenthMonthReportComponent', () => {
  let component: ThirtheenthMonthReportComponent;
  let fixture: ComponentFixture<ThirtheenthMonthReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ThirtheenthMonthReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThirtheenthMonthReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
