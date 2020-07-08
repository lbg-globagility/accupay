import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { range } from 'src/app/core/functions/dates';
import { range as rangeOfNumbers } from 'lodash';
import { CalendarDay } from 'src/app/calendars/shared/calendar-day';
import * as moment from 'moment';

interface CalendarCell {
  date: Date;
  calendarDay: CalendarDay;
}

@Component({
  selector: 'app-calendar-month',
  templateUrl: './calendar-month.component.html',
  styleUrls: ['./calendar-month.component.scss'],
})
export class CalendarMonthComponent implements OnInit {
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

  blankOffsets: [] = [];

  cells: CalendarCell[] = [];

  dates: Date[] = [];

  constructor() {}

  ngOnInit(): void {
    const { start, end } = this.getStartAndEndDate();

    this.blankOffsets = rangeOfNumbers(0, start.getDay());

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
    const start = new Date(y, m, 1);
    const end = new Date(y, m + 1, 0);

    return { start, end };
  }

  dayClicked(cell: CalendarCell) {
    this.dayClick.emit(cell.calendarDay);
  }
}
