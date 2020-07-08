import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditEmploymentPolicyComponent } from './edit-employment-policy.component';

describe('EditEmploymentPolicyComponent', () => {
  let component: EditEmploymentPolicyComponent;
  let fixture: ComponentFixture<EditEmploymentPolicyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditEmploymentPolicyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditEmploymentPolicyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
