import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StartPayrollDialogComponent } from './start-payroll-dialog.component';

describe('StartPayrollDialogComponent', () => {
  let component: StartPayrollDialogComponent;
  let fixture: ComponentFixture<StartPayrollDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [StartPayrollDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StartPayrollDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
