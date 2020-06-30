import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SssReportComponent } from './sss-report.component';

describe('SssReportComponent', () => {
  let component: SssReportComponent;
  let fixture: ComponentFixture<SssReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SssReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SssReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
