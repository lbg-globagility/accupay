import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Leave } from 'src/app/leaves/shared/leave';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';
import { SelfServiceLeaveService } from 'src/app/self-service/services';

@Component({
  selector: 'app-selfservice-leave-form',
  templateUrl: './selfservice-leave-form.component.html',
  styleUrls: ['./selfservice-leave-form.component.scss'],
})
export class SelfserviceLeaveFormComponent implements OnInit {
  @Input()
  leave: Leave;

  @Output()
  save: EventEmitter<Leave> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    startDate: [null, Validators.required],
    leaveType: [null, Validators.required],
    isWholeDay: [true],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    reason: [null],
  });

  leaveTypes: string[];

  constructor(
    private fb: FormBuilder,
    private service: SelfServiceLeaveService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {
    this.form.get('isWholeDay').valueChanges.subscribe((checked: boolean) => {
      this.disableTimeInputs(checked);
    });

    this.loadLeaveTypes();

    if (this.leave != null) {
      // this.form.get('employeeId').disable();
      this.form.patchValue(this.leave);
    }
    this.form.patchValue({
      startTime: this.timeParser.toInputTime(this.form.get('startTime').value),
      endTime: this.timeParser.toInputTime(this.form.get('endTime').value),
      isWholeDay:
        !this.form.get('startTime').value || !this.form.get('endTime').value,
    });
  }

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): Leave {
    const leave = cloneDeep(this.form.value as Leave);

    leave.startTime = this.timeParser.parse(
      moment(leave.startDate),
      leave.startTime
    );
    leave.endTime = this.timeParser.parse(
      moment(leave.startDate),
      leave.endTime
    );

    if (
      this.form.get('isWholeDay').value === true ||
      !leave.startTime ||
      !leave.endTime
    ) {
      leave.startTime = null;
      leave.endTime = null;
    }

    return leave;
  }

  private loadLeaveTypes(): void {
    this.service.getLeaveTypes().subscribe((data) => {
      this.leaveTypes = data;
    });
  }

  private disableTimeInputs(checked: boolean): void {
    if (checked) {
      this.form.get('startTime').disable();
      this.form.get('endTime').disable();
    } else {
      this.form.get('startTime').enable();
      this.form.get('endTime').enable();
    }
  }
}
