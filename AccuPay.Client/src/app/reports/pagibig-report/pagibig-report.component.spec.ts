import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PagibigReportComponent } from './pagibig-report.component';

describe('PagibigReportComponent', () => {
  let component: PagibigReportComponent;
  let fixture: ComponentFixture<PagibigReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PagibigReportComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PagibigReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
