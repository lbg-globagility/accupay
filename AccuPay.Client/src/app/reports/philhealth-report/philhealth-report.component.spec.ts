import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhilhealthReportComponent } from './philhealth-report.component';

describe('PhilhealthReportComponent', () => {
  let component: PhilhealthReportComponent;
  let fixture: ComponentFixture<PhilhealthReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhilhealthReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhilhealthReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
