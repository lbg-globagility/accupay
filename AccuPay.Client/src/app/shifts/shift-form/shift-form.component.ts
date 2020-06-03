import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Shift } from '../shared/shift';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import { cloneDeep } from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-shift-form',
  templateUrl: './shift-form.component.html',
  styleUrls: ['./shift-form.component.scss'],
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
    date: [null, [Validators.required]],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    breakStartTime: [null],
    breakLength: [0, Validators.required],
    isOffset: [false],
  });

  employees: Employee[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private timeParser: TimeParser
  ) {
    this.form
      .get('breakStartTime')
      .valueChanges.subscribe(() => {
        if (!this.form.get('breakStartTime').value) {
          this.form.get('breakLength').disable();
        } else {
          this.form.get('breakLength').enable();
        }
      });
  }

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
      breakStartTime: this.timeParser.toInputTime(
        this.form.get('breakStartTime').value
      ),
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

    shift.startTime = this.timeParser.parse(
      moment(shift.date),
      shift.startTime
    );

    shift.endTime = this.timeParser.parse(moment(shift.date), shift.endTime);

    shift.breakStartTime = this.timeParser.parse(
      moment(shift.date),
      shift.breakStartTime
    );

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
