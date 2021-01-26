import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import {
  CalendarListComponent,
  CalendarFormComponent,
  EditCalendarComponent,
  NewCalendarComponent,
  ViewCalendarComponent,
  CalendarDayDialogComponent,
} from 'src/app/calendars/components';
import { CalendarMonthComponent } from './components/calendar-month/calendar-month.component';

@NgModule({
  declarations: [
    CalendarListComponent,
    CalendarFormComponent,
    NewCalendarComponent,
    EditCalendarComponent,
    ViewCalendarComponent,
    CalendarDayDialogComponent,
    CalendarMonthComponent,
  ],
  imports: [SharedModule],
})
export class CalendarsModule {}
