import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { TimeParser } from 'src/app/core/shared/time-parser';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';

@Component({
  selector: 'app-leave-form',
  templateUrl: './leave-form.component.html',
  styleUrls: ['./leave-form.component.scss'],
})
export class LeaveFormComponent implements OnInit {
  @Input()
  leave: Leave;

  @Output()
  save: EventEmitter<Leave> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, [Validators.required]],
    startDate: [null, [Validators.required]],
    leaveType: [null, Validators.required],
    isWholeDay: [true],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    status: [null, Validators.required],
    reason: [null],
    comments: [null],
  });

  computedTotalLeave: number;

  employees: Employee[];
  leaveTypes: string[];
  statusList: string[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private leaveService: LeaveService,
    private timeParser: TimeParser
  ) {
    this.form.get('isWholeDay').valueChanges.subscribe((checked: boolean) => {
      this.disableTimeInputs(checked);
    });
  }

  ngOnInit(): void {
    this.loadLeaveTypes();
    this.loadLeaveStatusList();

    if (this.leave != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.leave);
    } else {
      this.loadEmployees();
    }
    this.form.patchValue({
      startTime: this.timeParser.toInputTime(this.form.get('startTime').value),
      endTime: this.timeParser.toInputTime(this.form.get('endTime').value),
      isWholeDay:
        !this.form.get('startTime').value || !this.form.get('endTime').value,
    });
  }

  private loadLeaveTypes(): void {
    this.leaveService.getLeaveTypes().subscribe((data) => {
      this.leaveTypes = data;
    });
  }

  private loadLeaveStatusList(): void {
    this.leaveService.getStatusList().subscribe((data) => {
      this.statusList = data;
    });
  }

  private loadEmployees(): void {
    const options = new PageOptions(0, 1000, null, null);

    this.employeeService.getList(options).subscribe((data) => {
      this.employees = data.items;
    });
  }

  onSave(): void {
    if (!this.form.valid) {
      return;
    }

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

    this.save.emit(leave);
  }

  onCancel(): void {
    this.cancel.emit();
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
