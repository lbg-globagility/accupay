import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

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

  filteredEmployees: Observable<Employee[]>;

  statusList: string[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private overtimeService: OvertimeService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {

    this.filteredEmployees = this.form.get('employeeId').valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );

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

  displayEmployee = (employeeId: number) => {
    const employee = this.employees?.find((t) => t.id === employeeId);
    if (employee == null) {
      return null;
    }

    return `${employee.employeeNo} - ${employee.lastName} ${employee.firstName}`;
  };

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
