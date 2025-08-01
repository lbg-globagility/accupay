import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { range } from 'src/app/core/functions/dates';
import { range as rangeOfNumbers } from 'lodash';
import { CalendarDay } from 'src/app/calendars/shared/calendar-day';
import * as moment from 'moment';
import { PermissionTypes } from 'src/app/core/auth';

interface CalendarCell {
  date: Date;
  calendarDay: CalendarDay;
}

@Component({
  selector: 'app-calendar-month',
  templateUrl: './calendar-month.component.html',
  styleUrls: ['./calendar-month.component.scss'],
  host: {
    class: 'block mat-elevation-z1 p-4 bg-white',
  },
})
export class CalendarMonthComponent implements OnInit {
  readonly PermissionTypes = PermissionTypes;

  @Input()
  month: Date = new Date();

  @Input()
  calendarDays: CalendarDay[] = [];

  @Output()
  dayClick: EventEmitter<CalendarDay> = new EventEmitter();

  readonly daysOfWeek: string[] = [
    'Sun',
    'Mon',
    'Tue',
    'Wed',
    'Thu',
    'Fri',
    'Sat',
  ];

  blankCells: [] = [];

  cells: CalendarCell[] = [];

  ngOnInit(): void {
    const { start, end } = this.getStartAndEndDate();

    this.blankCells = rangeOfNumbers(0, start.getDay());

    this.cells = range(start, end).map((date) => {
      const cell: CalendarCell = {
        date,
        calendarDay: this.calendarDays.find(
          (t) => moment(t.date).toDate().getDate() === date.getDate()
        ),
      };

      return cell;
    });
  }

  getStartAndEndDate(): { start: Date; end: Date } {
    const y = this.month.getFullYear();
    const m = this.month.getMonth();
    const start = new Date(Date.UTC(y, m, 1));
    const end = new Date(Date.UTC(y, m + 1, 0));

    return { start, end };
  }

  dayClicked(cell: CalendarCell): void {
    let { calendarDay } = cell;
    if (calendarDay == null) {
      calendarDay = {
        id: null,
        date: cell.date,
        dayType: null,
        description: null,
      };
    }

    this.dayClick.emit(calendarDay);
  }
}
