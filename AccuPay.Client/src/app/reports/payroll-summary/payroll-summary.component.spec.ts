import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollSummaryComponent } from './payroll-summary.component';

describe('PayrollSummaryComponent', () => {
  let component: PayrollSummaryComponent;
  let fixture: ComponentFixture<PayrollSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
