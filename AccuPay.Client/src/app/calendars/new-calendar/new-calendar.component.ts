import { Component, OnInit } from '@angular/core';
import { CalendarService } from 'src/app/calendars/service/calendar.service';
import { Calendar } from 'src/app/calendars/shared/calendar';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-new-calendar',
  templateUrl: './new-calendar.component.html',
  styleUrls: ['./new-calendar.component.scss'],
})
export class NewCalendarComponent implements OnInit {
  constructor(
    private calendarService: CalendarService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSave(calendar: Calendar) {
    this.calendarService.create(calendar).subscribe({
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
      text: 'Successfully created a new calendar!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
