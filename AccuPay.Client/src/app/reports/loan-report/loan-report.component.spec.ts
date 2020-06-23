import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanReportComponent } from './loan-report.component';

describe('LoanReportComponent', () => {
  let component: LoanReportComponent;
  let fixture: ComponentFixture<LoanReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoanReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoanReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
