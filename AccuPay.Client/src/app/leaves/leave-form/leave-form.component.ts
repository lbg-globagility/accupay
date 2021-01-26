import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

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

  employees: Employee[];

  filteredEmployees: Observable<Employee[]>;

  leaveTypes: string[];
  statusList: string[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private leaveService: LeaveService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {
    this.form.get('isWholeDay').valueChanges.subscribe((checked: boolean) => {
      this.disableTimeInputs(checked);
    });

    this.filteredEmployees = this.form.get('employeeId').valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );

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

  displayEmployee = (employeeId: number) => {
    const employee = this.employees?.find((t) => t.id === employeeId);
    if (employee == null) {
      return null;
    }

    return `${employee.employeeNo} - ${employee.lastName} ${employee.firstName}`;
  };

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
    this.employeeService.getAll().subscribe((data) => {
      this.employees = data;
    });
  }

  private _filter(value: string) {
    console.log('filter');

    if (typeof value !== 'string') {
      return;
    }

    const filterValue = value.toLowerCase();

    return this.employees?.filter(
      (employee) =>
        employee.employeeNo?.toLowerCase().includes(filterValue) ||
        employee.firstName?.toLowerCase().includes(filterValue) ||
        employee.lastName?.toLowerCase().includes(filterValue)
    );
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
