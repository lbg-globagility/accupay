import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveOvertimeComponent } from './selfserve-overtime.component';

describe('SelfserveOvertimeComponent', () => {
  let component: SelfserveOvertimeComponent;
  let fixture: ComponentFixture<SelfserveOvertimeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveOvertimeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveOvertimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
