import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanReportByemployeeComponent } from './loan-report-byemployee.component';

describe('LoanReportByemployeeComponent', () => {
  let component: LoanReportByemployeeComponent;
  let fixture: ComponentFixture<LoanReportByemployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoanReportByemployeeComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoanReportByemployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
