import { Component, OnInit } from '@angular/core';
import { Calendar } from 'src/app/calendars/shared/calendar';
import { CalendarService } from 'src/app/calendars/service/calendar.service';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-edit-calendar',
  templateUrl: './edit-calendar.component.html',
  styleUrls: ['./edit-calendar.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditCalendarComponent implements OnInit {
  calendarId: number = +this.route.snapshot.paramMap.get('id');

  calendar: Calendar;

  constructor(
    private calendarService: CalendarService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.calendarService
      .getById(this.calendarId)
      .subscribe((calendar) => (this.calendar = calendar));
  }

  onSave(calendar: Calendar) {
    console.log(calendar);

    this.calendarService.update(this.calendarId, calendar).subscribe({
      next: () => {
        this.displaySuccess();
        this.router.navigate(['calendars']);
      },
    });
  }

  onCancel() {
    this.router.navigate(['calendars']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated a new calendar!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
