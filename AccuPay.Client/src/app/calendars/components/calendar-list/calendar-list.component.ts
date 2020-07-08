import { Component, OnInit } from '@angular/core';
import { CalendarService } from 'src/app/calendars/service/calendar.service';
import { Calendar } from 'src/app/calendars/shared/calendar';

@Component({
  selector: 'app-calendar-list',
  templateUrl: './calendar-list.component.html',
  styleUrls: ['./calendar-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class CalendarListComponent implements OnInit {
  readonly displayedColumns: string[] = ['name', 'actions'];

  calendars: Calendar[];

  constructor(private calendarService: CalendarService) {}

  ngOnInit(): void {
    this.loadCalendars();
  }

  private loadCalendars() {
    this.calendarService.list().subscribe((calendars) => {
      this.calendars = calendars;
    });
  }
}
