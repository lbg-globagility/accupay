import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { CalendarListComponent } from './calendar-list/calendar-list.component';
import { CalendarFormComponent } from './calendar-form/calendar-form.component';
import { NewCalendarComponent } from './new-calendar/new-calendar.component';
import { EditCalendarComponent } from './edit-calendar/edit-calendar.component';
import { ViewCalendarComponent } from './view-calendar/view-calendar.component';
import { CalendarDayDialogComponent } from './calendar-day-dialog/calendar-day-dialog.component';

@NgModule({
  declarations: [
    CalendarListComponent,
    CalendarFormComponent,
    NewCalendarComponent,
    EditCalendarComponent,
    ViewCalendarComponent,
    CalendarDayDialogComponent,
  ],
  imports: [SharedModule],
})
export class CalendarsModule {}
