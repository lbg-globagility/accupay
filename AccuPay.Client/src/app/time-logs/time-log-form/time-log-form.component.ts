import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TimeLog } from '../shared/time-log';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { TimeLogService } from '../time-log.service';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import { PageOptions } from 'src/app/core/shared/page-options';
import { cloneDeep } from 'lodash';
import * as moment from 'moment';
import { Branch } from 'src/app/branches/shared/branch';
import { BranchService } from 'src/app/branches/services/branch.service';

@Component({
  selector: 'app-time-log-form',
  templateUrl: './time-log-form.component.html',
  styleUrls: ['./time-log-form.component.scss']
})
export class TimeLogFormComponent implements OnInit {
  @Input()
  timeLog: TimeLog;

  @Output()
  save: EventEmitter<TimeLog> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, Validators.required],
    date: [null, Validators.required],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    branchId: [null, Validators.required],
  });

  employees: Employee[];
  branches: Branch[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private branchService: BranchService,
    private timeLogService: TimeLogService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {
    this.loadBranches();

    if (this.timeLog != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.timeLog);
    } else {
      this.loadEmployees();
    }
    this.form.patchValue({
      endTime: this.timeParser.toInputTime(this.form.get('endTime').value),
      startTime: this.timeParser.toInputTime(this.form.get('startTime').value),
    });
  }

  loadBranches() {
    this.branchService.list().subscribe((branches) => {
      this.branches = branches;
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

    const timeLog = cloneDeep(this.form.value as TimeLog);

    timeLog.startTime = this.timeParser.parse(
      moment(timeLog.date),
      timeLog.startTime
    );

    timeLog.endTime = this.timeParser.parse(moment(timeLog.date), timeLog.endTime);

    if (!timeLog.startTime || !timeLog.endTime) {
      timeLog.startTime = null;
      timeLog.endTime = null;
    }
    
    this.save.emit(timeLog);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
