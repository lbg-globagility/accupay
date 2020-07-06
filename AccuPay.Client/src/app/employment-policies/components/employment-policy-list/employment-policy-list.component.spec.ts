import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploymentPolicyListComponent } from './employment-policy-list.component';

describe('EmploymentPolicyListComponent', () => {
  let component: EmploymentPolicyListComponent;
  let fixture: ComponentFixture<EmploymentPolicyListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmploymentPolicyListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploymentPolicyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
