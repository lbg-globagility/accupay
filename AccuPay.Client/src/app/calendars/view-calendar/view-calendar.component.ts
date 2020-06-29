import { Component, OnInit } from '@angular/core';
import { CalendarService } from 'src/app/calendars/service/calendar.service';
import { CalendarDay } from 'src/app/calendars/shared/calendar-day';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CalendarDayDialogComponent } from 'src/app/calendars/calendar-day-dialog/calendar-day-dialog.component';
import { DayType } from 'src/app/calendars/shared/day-type';
import { filter } from 'rxjs/operators';
import { Calendar } from 'src/app/calendars/shared/calendar';

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

  year: number = new Date().getFullYear();

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
    this.loadDays();
    this.loadDayTypes();
  }

  private loadCalendar() {
    this.calendarService
      .getById(this.calendarId)
      .subscribe((calendar) => (this.calendar = calendar));
  }

  private loadDays() {
    this.calendarService
      .getDays(this.calendarId, this.year)
      .subscribe((calendarDays) => (this.calendarDays = calendarDays));
  }

  private loadDayTypes() {
    this.calendarService
      .getDayTypes()
      .subscribe((dayTypes) => (this.dayTypes = dayTypes));
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
}
