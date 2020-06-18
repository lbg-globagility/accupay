import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollResultDetailsComponent } from './payroll-result-details.component';

describe('PayrollResultDetailsComponent', () => {
  let component: PayrollResultDetailsComponent;
  let fixture: ComponentFixture<PayrollResultDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PayrollResultDetailsComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollResultDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
