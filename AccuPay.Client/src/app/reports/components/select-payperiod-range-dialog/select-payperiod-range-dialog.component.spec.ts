import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectPayperiodRangeDialogComponent } from './select-payperiod-range-dialog.component';

describe('SelectPayperiodDialogComponent', () => {
  let component: SelectPayperiodRangeDialogComponent;
  let fixture: ComponentFixture<SelectPayperiodRangeDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelectPayperiodRangeDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectPayperiodRangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
