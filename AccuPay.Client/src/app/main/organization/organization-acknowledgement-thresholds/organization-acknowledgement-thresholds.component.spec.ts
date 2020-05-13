import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationAcknowledgementThresholdsComponent } from './organization-acknowledgement-thresholds.component';

describe('OrganizationAcknowledgementThresholdsComponent', () => {
  let component: OrganizationAcknowledgementThresholdsComponent;
  let fixture: ComponentFixture<OrganizationAcknowledgementThresholdsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [OrganizationAcknowledgementThresholdsComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(
      OrganizationAcknowledgementThresholdsComponent
    );
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
