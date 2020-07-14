import { Component, OnInit } from '@angular/core';
import { CalendarService } from 'src/app/calendars/service/calendar.service';
import { CalendarDay } from 'src/app/calendars/shared/calendar-day';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CalendarDayDialogComponent } from 'src/app/calendars/components/calendar-day-dialog/calendar-day-dialog.component';
import { DayType } from 'src/app/calendars/shared/day-type';
import { filter, tap } from 'rxjs/operators';
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

  calendarId: number = +this.route.snapshot.paramMap.get('id');

  selectedYear: number = new Date().getFullYear();

  calendar: Calendar;

  calendarDays: CalendarDay[];

  calendarMonths: CalendarMonth[] = [];

  dayTypes: DayType[];

  changedCalendarDays: CalendarDay[] = [];

  constructor(
    private calendarService: CalendarService,
    private route: ActivatedRoute,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadDayTypes();

    this.loadCalendar();
    this.loadCalendarDays().subscribe(() => this.createListOfMonths());
  }

  changeDay(calendarDay: CalendarDay): void {
    this.dialog
      .open(CalendarDayDialogComponent, {
        data: { calendarDay, dayTypes: this.dayTypes },
      })
      .afterClosed()
      .pipe(filter((t) => t != null))
      .subscribe((result: CalendarDay) => {
        this.addToChangeTracker(result);
        this.updateCalendarDayList(result);
      });
  }

  onSave(): void {
    this.calendarService
      .updateDays(this.calendarId, this.changedCalendarDays)
      .subscribe({
        next: () => {
          // After save, clear the change tracker
          this.changedCalendarDays = [];
        },
      });
  }

  goToNextYear(): void {
    this.selectedYear = this.selectedYear + 1;
    this.loadCalendarDays().subscribe(() => this.createListOfMonths());
  }

  goToPreviousYear(): void {
    this.selectedYear = this.selectedYear - 1;
    this.loadCalendarDays().subscribe(() => this.createListOfMonths());
  }

  private createListOfMonths(): void {
    const monthsPerYear = 12;
    const calendarMonths = [];
    for (let i = 0; i < monthsPerYear; i++) {
      const month = new Date(this.selectedYear, i, 1);

      const days = this.calendarDays.filter(
        (t) => moment(t.date).toDate().getMonth() === month.getMonth()
      );

      const calendarMonth = { month, days };

      calendarMonths.push(calendarMonth);
    }

    this.calendarMonths = calendarMonths;
  }

  private addToChangeTracker(calendarDay: CalendarDay): void {
    const foundIndex = this.changedCalendarDays.findIndex(
      (t) => t.id === calendarDay.id
    );
    // If the calendar day was found, then update the calendar day, otherwise push the new calendar day
    if (foundIndex > -1) {
      this.changedCalendarDays[foundIndex] = calendarDay;
    } else {
      this.changedCalendarDays.push(calendarDay);
    }
  }

  private updateCalendarDayList(calendarDay: CalendarDay): void {
    // Create a new list of calendar days to make the calendar rerender
    const index = this.calendarDays.findIndex(
      (t) => t.date === calendarDay.date
    );

    const newList = this.calendarDays.slice(0);
    this.calendarDays = newList;

    if (index > 0) {
      newList[index] = calendarDay;
    } else {
      newList.push(calendarDay);
    }

    this.createListOfMonths();
  }

  private loadCalendar(): void {
    this.calendarService
      .getById(this.calendarId)
      .subscribe((calendar) => (this.calendar = calendar));
  }

  private loadCalendarDays(): Observable<CalendarDay[]> {
    return this.calendarService
      .getDays(this.calendarId, this.selectedYear)
      .pipe(tap((calendarDays) => (this.calendarDays = calendarDays)));
  }

  private loadDayTypes(): void {
    this.calendarService
      .getDayTypes()
      .subscribe((dayTypes) => (this.dayTypes = dayTypes));
  }
}
