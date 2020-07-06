import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewEmploymentPolicyComponent } from './new-employment-policy.component';

describe('NewEmploymentPolicyComponent', () => {
  let component: NewEmploymentPolicyComponent;
  let fixture: ComponentFixture<NewEmploymentPolicyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewEmploymentPolicyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewEmploymentPolicyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
