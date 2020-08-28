import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveOvertimeFormComponent } from './selfserve-overtime-form.component';

describe('SelfserveOvertimeFormComponent', () => {
  let component: SelfserveOvertimeFormComponent;
  let fixture: ComponentFixture<SelfserveOvertimeFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveOvertimeFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveOvertimeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
