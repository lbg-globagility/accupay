import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeLogImportResultComponent } from './time-log-import-result.component';

describe('TimeLogImportResultComponent', () => {
  let component: TimeLogImportResultComponent;
  let fixture: ComponentFixture<TimeLogImportResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TimeLogImportResultComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeLogImportResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
