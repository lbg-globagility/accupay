import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';

@Component({
  selector: 'app-overtime-form',
  templateUrl: './overtime-form.component.html',
  styleUrls: ['./overtime-form.component.scss'],
})
export class OvertimeFormComponent implements OnInit {
  @Input()
  overtime: Overtime;

  @Output()
  save: EventEmitter<Overtime> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, [Validators.required]],
    startDate: [null, [Validators.required]],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    status: [null, Validators.required],
    reason: [null],
    comments: [null],
  });

  employees: Employee[];
  statusList: string[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private overtimeService: OvertimeService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {
    this.loadOvertimeStatusList();

    if (this.overtime != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.overtime);
    } else {
      this.loadEmployees();
    }
    this.form.patchValue({
      startTime: this.timeParser.toInputTime(this.form.get('startTime').value),
      endTime: this.timeParser.toInputTime(this.form.get('endTime').value),
    });
  }

  private loadOvertimeStatusList(): void {
    this.overtimeService.getStatusList().subscribe((data) => {
      this.statusList = data;
    });
  }

  private loadEmployees(): void {
    this.employeeService.getAll().subscribe((data) => {
      this.employees = data;
    });
  }

  onSave(): void {
    if (!this.form.valid) {
      return;
    }

    const overtime = cloneDeep(this.form.value as Overtime);

    console.log(overtime);
    overtime.startTime = this.timeParser.parse(
      moment(overtime.startDate),
      overtime.startTime
    );
    overtime.endTime = this.timeParser.parse(
      moment(overtime.startDate),
      overtime.endTime
    );
    console.log(overtime);

    if (!overtime.startTime || !overtime.endTime) {
      overtime.startTime = null;
      overtime.endTime = null;
    }

    this.save.emit(overtime);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
