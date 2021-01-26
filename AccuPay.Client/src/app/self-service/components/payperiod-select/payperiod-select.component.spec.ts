import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayperiodSelectComponent } from './payperiod-select.component';

describe('PayperiodSelectComponent', () => {
  let component: PayperiodSelectComponent;
  let fixture: ComponentFixture<PayperiodSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayperiodSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayperiodSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
