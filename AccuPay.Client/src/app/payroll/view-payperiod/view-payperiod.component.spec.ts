import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewPayPeriodComponent } from './view-payperiod.component';

describe('ViewPayPeriodComponent', () => {
  let component: ViewPayPeriodComponent;
  let fixture: ComponentFixture<ViewPayPeriodComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ViewPayPeriodComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewPayPeriodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
