import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfServiceTimesheetsComponent } from './self-service-timesheets.component';

describe('SelfServiceTimesheetsComponent', () => {
  let component: SelfServiceTimesheetsComponent;
  let fixture: ComponentFixture<SelfServiceTimesheetsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfServiceTimesheetsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfServiceTimesheetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
