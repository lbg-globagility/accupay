import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarDayDialogComponent } from './calendar-day-dialog.component';

describe('CalendarDayDialogComponent', () => {
  let component: CalendarDayDialogComponent;
  let fixture: ComponentFixture<CalendarDayDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarDayDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarDayDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
