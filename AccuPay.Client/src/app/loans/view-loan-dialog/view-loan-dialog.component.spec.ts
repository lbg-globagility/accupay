import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewLoanDialogComponent } from './view-loan-dialog.component';

describe('ViewLoanDialogComponent', () => {
  let component: ViewLoanDialogComponent;
  let fixture: ComponentFixture<ViewLoanDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewLoanDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewLoanDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
