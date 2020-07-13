import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectPayperiodDialogComponent } from './select-payperiod-dialog.component';

describe('SelectPayperiodDialogComponent', () => {
  let component: SelectPayperiodDialogComponent;
  let fixture: ComponentFixture<SelectPayperiodDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelectPayperiodDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectPayperiodDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
