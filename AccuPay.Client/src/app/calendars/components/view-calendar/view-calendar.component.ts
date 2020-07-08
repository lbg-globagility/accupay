import { Component, OnInit } from '@angular/core';
import { CalendarService } from 'src/app/calendars/service/calendar.service';
import { CalendarDay } from 'src/app/calendars/shared/calendar-day';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CalendarDayDialogComponent } from 'src/app/calendars/components/calendar-day-dialog/calendar-day-dialog.component';
import { DayType } from 'src/app/calendars/shared/day-type';
import { filter, tap, map } from 'rxjs/operators';
import { Calendar } from 'src/app/calendars/shared/calendar';
import { Observable } from 'rxjs';
import * as moment from 'moment';

interface CalendarMonth {
  month: Date;
  days: CalendarDay[];
}

@Component({
  selector: 'app-view-calendar',
  templateUrl: './view-calendar.component.html',
  styleUrls: ['./view-calendar.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewCalendarComponent implements OnInit {
  readonly displayedColumns: string[] = ['date', 'description'];

  currentYear: number = 2020;

  calendarId: number = +this.route.snapshot.paramMap.get('id');

  year: number = new Date().getFullYear();

  months: Date[] = [];

  calendarMonths: CalendarMonth[] = [];

  calendarDays: CalendarDay[];

  calendar: Calendar;

  dayTypes: DayType[];

  changedCalendarDays: CalendarDay[] = [];

  constructor(
    private calendarService: CalendarService,
    private route: ActivatedRoute,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadCalendar();
    this.loadDayTypes();
    this.loadCalendarDays().subscribe(() => this.createListOfMonths());
  }

  private createListOfMonths(): void {
    const monthsPerYear = 12;
    const months = [];
    const da = [];
    for (let i = 0; i < monthsPerYear; i++) {
      const month = new Date(this.year, i, 1);

      const days = this.calendarDays.filter(
        (t) => moment(t.date).toDate().getMonth() === month.getMonth()
      );

      const da2 = {
        month,
        days,
      };

      da.push(da2);

      months.push(month);
    }
    this.months = months;
    this.calendarMonths = da;
  }

  changeDay(calendarDay: CalendarDay) {
    this.dialog
      .open(CalendarDayDialogComponent, {
        data: { calendarDay, dayTypes: this.dayTypes },
      })
      .afterClosed()
      .pipe(filter((t) => t != null))
      .subscribe((result) => {
        // Add changed day to change tracker
        const foundIndex = this.changedCalendarDays.findIndex(
          (t) => t.id === result.id
        );
        if (foundIndex > -1) {
          this.changedCalendarDays[foundIndex] = result;
        } else {
          this.changedCalendarDays.push(result);
        }

        // Update list of calendar days
        const index = this.calendarDays.findIndex((t) => t.id === result.id);
        const newList = this.calendarDays.slice(0);
        newList[index] = result;
        this.calendarDays = newList;
      });
  }

  onSave() {
    this.calendarService
      .updateDays(this.calendarId, this.changedCalendarDays)
      .subscribe({
        next: () => {
          this.changedCalendarDays = [];
        },
      });
  }

  private loadCalendar(): void {
    this.calendarService
      .getById(this.calendarId)
      .subscribe((calendar) => (this.calendar = calendar));
  }

  private loadCalendarDays(): Observable<CalendarDay[]> {
    return this.calendarService
      .getDays(this.calendarId, this.year)
      .pipe(tap((calendarDays) => (this.calendarDays = calendarDays)));
  }

  private loadDayTypes(): void {
    this.calendarService
      .getDayTypes()
      .subscribe((dayTypes) => (this.dayTypes = dayTypes));
  }
}
