import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CalendarDay } from 'src/app/calendars/shared/calendar-day';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DayType } from 'src/app/calendars/shared/day-type';

@Component({
  selector: 'app-calendar-day-dialog',
  templateUrl: './calendar-day-dialog.component.html',
  styleUrls: ['./calendar-day-dialog.component.scss'],
})
export class CalendarDayDialogComponent implements OnInit {
  dayTypes: DayType[] = [];

  calendarDay: CalendarDay;

  form: FormGroup = this.fb.group({
    dayType: [],
    description: [],
  });

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialogRef<CalendarDayDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.calendarDay = data.calendarDay;
    this.dayTypes = data.dayTypes;
  }

  ngOnInit(): void {
    this.form.patchValue(this.calendarDay);
  }

  onOk() {
    const value = this.form.value;
    const oldValue = {
      dayType: this.calendarDay.dayType,
      description: this.calendarDay.description,
    };

    const hasChanged = JSON.stringify(value) !== JSON.stringify(oldValue);

    if (hasChanged) {
      this.dialog.close({
        id: this.calendarDay.id,
        date: this.calendarDay.date,
        dayType: value.dayType,
        description: value.description,
      });
    } else {
      this.dialog.close(null);
    }
  }
}
