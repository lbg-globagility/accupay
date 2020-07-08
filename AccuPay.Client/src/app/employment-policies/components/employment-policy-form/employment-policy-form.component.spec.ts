import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploymentPolicyFormComponent } from './employment-policy-form.component';

describe('EmploymentPolicyFormComponent', () => {
  let component: EmploymentPolicyFormComponent;
  let fixture: ComponentFixture<EmploymentPolicyFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmploymentPolicyFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploymentPolicyFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
