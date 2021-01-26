import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustmentsBreakdownComponent } from './adjustments-breakdown.component';

describe('AdjustmentsBreakdownComponent', () => {
  let component: AdjustmentsBreakdownComponent;
  let fixture: ComponentFixture<AdjustmentsBreakdownComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AdjustmentsBreakdownComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdjustmentsBreakdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
