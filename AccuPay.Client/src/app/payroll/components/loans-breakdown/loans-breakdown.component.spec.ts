import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoansBreakdownComponent } from './loans-breakdown.component';

describe('LoansBreakdownComponent', () => {
  let component: LoansBreakdownComponent;
  let fixture: ComponentFixture<LoansBreakdownComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoansBreakdownComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoansBreakdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
