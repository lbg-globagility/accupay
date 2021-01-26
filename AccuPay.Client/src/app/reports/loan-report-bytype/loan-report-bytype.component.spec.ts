import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanReportBytypeComponent } from './loan-report-bytype.component';

describe('LoanReportBytypeComponent', () => {
  let component: LoanReportBytypeComponent;
  let fixture: ComponentFixture<LoanReportBytypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoanReportBytypeComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoanReportBytypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
