import { Component, OnInit, Input } from '@angular/core';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import { cloneDeep } from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-selfservice-overtime-form',
  templateUrl: './selfservice-overtime-form.component.html',
  styleUrls: ['./selfservice-overtime-form.component.scss'],
})
export class SelfserviceOvertimeFormComponent implements OnInit {
  @Input()
  overtime: Overtime;

  form: FormGroup = this.fb.group({
    startDate: [null, Validators.required],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    reason: [null],
  });

  constructor(private fb: FormBuilder, private timeParser: TimeParser) {}

  ngOnInit(): void {}

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): Overtime {
    const overtime = cloneDeep(this.form.value as Overtime);

    overtime.startTime = this.timeParser.parse(
      moment(overtime.startDate),
      overtime.startTime
    );
    overtime.endTime = this.timeParser.parse(
      moment(overtime.startDate),
      overtime.endTime
    );

    return overtime;
  }
}
