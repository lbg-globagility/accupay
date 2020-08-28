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

  @Output()
  save: EventEmitter<Overtime> = new EventEmitter();

  @Output()
  cancel: EventEmitter<Overtime> = new EventEmitter();

  savingState: LoadingState = new LoadingState();

  form: FormGroup = this.fb.group({
    startTime: [null],
    endTime: [null],
    startDate: [null, [Validators.required]],
    endDate: [null],
    status: ['Pending', [Validators.required]],
    reason: [null],
    comments: [null],
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
