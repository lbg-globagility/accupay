import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { LoadingState } from 'src/app/core/states/loading-state';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { FormBuilder, Form, FormGroup, Validators } from '@angular/forms';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import { cloneDeep } from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-selfserve-overtime-form',
  templateUrl: './selfserve-overtime-form.component.html',
  styleUrls: ['./selfserve-overtime-form.component.scss'],
})
export class SelfserveOvertimeFormComponent implements OnInit {
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
