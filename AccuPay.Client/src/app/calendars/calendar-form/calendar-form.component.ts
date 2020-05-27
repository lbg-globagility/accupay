import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Calendar } from 'src/app/calendars/shared/calendar';
import { CalendarService } from 'src/app/calendars/service/calendar.service';

@Component({
  selector: 'app-calendar-form',
  templateUrl: './calendar-form.component.html',
  styleUrls: ['./calendar-form.component.scss'],
})
export class CalendarFormComponent implements OnInit {
  @Input()
  calendar: Calendar;

  @Output()
  save: EventEmitter<Calendar> = new EventEmitter();

  @Output()
  cancel: EventEmitter<void> = new EventEmitter();

  calendars: Calendar[];

  form: FormGroup = this.fb.group({
    name: [, [Validators.required]],
    copiedCalendarId: [],
  });

  constructor(
    private fb: FormBuilder,
    private calendarService: CalendarService
  ) {}

  ngOnInit(): void {
    if (this.calendar) {
      this.form.patchValue(this.calendar);
    }

    this.loadCalendars();
  }

  loadCalendars() {
    this.calendarService
      .list()
      .subscribe((calendars) => (this.calendars = calendars));
  }

  onSave() {
    if (!this.form.valid) {
      return;
    }

    const value = this.form.value;
    this.save.emit(value);
  }

  onCancel() {
    this.cancel.emit();
  }
}
