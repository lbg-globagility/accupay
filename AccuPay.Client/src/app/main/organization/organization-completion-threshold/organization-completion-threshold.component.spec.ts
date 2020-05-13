import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationCompletionThresholdComponent } from './organization-completion-threshold.component';

describe('OrganizationCompletionThresholdComponent', () => {
  let component: OrganizationCompletionThresholdComponent;
  let fixture: ComponentFixture<OrganizationCompletionThresholdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationCompletionThresholdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationCompletionThresholdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
