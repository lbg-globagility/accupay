import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectPayperiodRangeComponent } from './select-payperiod-range.component';

describe('SelectPayperiodRangeComponent', () => {
  let component: SelectPayperiodRangeComponent;
  let fixture: ComponentFixture<SelectPayperiodRangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectPayperiodRangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectPayperiodRangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
