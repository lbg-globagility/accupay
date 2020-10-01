import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserviceOvertimeFormComponent } from './selfservice-overtime-form.component';

describe('SelfserviceOvertimeFormComponent', () => {
  let component: SelfserviceOvertimeFormComponent;
  let fixture: ComponentFixture<SelfserviceOvertimeFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelfserviceOvertimeFormComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceOvertimeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
