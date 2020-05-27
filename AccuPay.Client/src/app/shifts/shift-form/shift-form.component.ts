import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Shift } from '../shared/shift';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { ShiftService } from '../shift.service';
import { TimeParser } from 'src/app/core/shared/time-parser';
import { PageOptions } from 'src/app/core/shared/page-options';
import { cloneDeep } from 'lodash';
import * as moment from 'moment';
import { strict } from 'assert';

@Component({
  selector: 'app-shift-form',
  templateUrl: './shift-form.component.html',
  styleUrls: ['./shift-form.component.scss']
})
export class ShiftFormComponent implements OnInit {
  @Input()
  shift: Shift;

  @Output()
  save: EventEmitter<Shift> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, [Validators.required]],
    dateSched: [null, [Validators.required]],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    breakStartTime: [null, Validators.required],
    breakLength: [null, Validators.required],
    isRestDay: [null],
    shiftHours: [null],
    workHours: [null]
  });

  employees: Employee[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private shiftService: ShiftService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {

    if (this.shift != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.shift);
    } else {
      this.loadEmployees();
    }
    this.form.patchValue({
      endTime: this.timeParser.toInputTime(this.form.get('endTime').value),
      startTime: this.timeParser.toInputTime(this.form.get('startTime').value),
      breakStartTime: this.timeParser.toInputTime(this.form.get('breakStartTime').value),
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

    const shift = cloneDeep(this.form.value as Shift);

    console.log(shift);
    shift.startTime = this.timeParser.parse(
      moment(shift.dateSched),
      shift.startTime
    );
    shift.endTime = this.timeParser.parse(
      moment(shift.dateSched),
      shift.endTime
    );
    shift.breakStartTime = this.timeParser.parse(
      moment(shift.dateSched),
      shift.breakStartTime
    );
    console.log(shift);

    if (!shift.startTime || !shift.endTime) {
      shift.startTime = null;
      shift.endTime = null;
    }

    this.save.emit(shift);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
