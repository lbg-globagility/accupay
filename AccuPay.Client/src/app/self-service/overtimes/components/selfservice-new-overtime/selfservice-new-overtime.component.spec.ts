import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfServiceNewOvertimeComponent } from './selfserve-overtime.component';

describe('SelfServiceNewOvertimeComponent', () => {
  let component: SelfServiceNewOvertimeComponent;
  let fixture: ComponentFixture<SelfServiceNewOvertimeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelfServiceNewOvertimeComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfServiceNewOvertimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
