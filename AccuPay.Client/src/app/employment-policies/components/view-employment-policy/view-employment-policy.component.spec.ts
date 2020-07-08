import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewEmploymentPolicyComponent } from './view-employment-policy.component';

describe('ViewEmploymentPolicyComponent', () => {
  let component: ViewEmploymentPolicyComponent;
  let fixture: ComponentFixture<ViewEmploymentPolicyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewEmploymentPolicyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewEmploymentPolicyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
