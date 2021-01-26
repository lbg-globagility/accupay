import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { OfficialBusiness } from '../shared/official-business';
import { OfficialBusinessService } from '../official-business.service';

@Component({
  selector: 'app-official-business-form',
  templateUrl: './official-business-form.component.html',
  styleUrls: ['./official-business-form.component.scss'],
})
export class OfficialBusinessFormComponent implements OnInit {
  @Input()
  officialBusiness: OfficialBusiness;

  @Output()
  save: EventEmitter<OfficialBusiness> = new EventEmitter();

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

  leaveTypes: string[];
  statusList: string[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private officialBusinessService: OfficialBusinessService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {
    this.filteredEmployees = this.form.get('employeeId').valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );

    this.loadOfficialBusinessStatusList();

    if (this.officialBusiness != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.officialBusiness);
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

  get value(): OfficialBusiness {
    const officialBusiness = cloneDeep(this.form.value as OfficialBusiness);

    officialBusiness.startTime = this.timeParser.parse(
      moment(officialBusiness.startDate),
      officialBusiness.startTime
    );
    officialBusiness.endTime = this.timeParser.parse(
      moment(officialBusiness.startDate),
      officialBusiness.endTime
    );

    return officialBusiness;
  }

  displayEmployee = (employeeId: number) => {
    const employee = this.employees?.find((t) => t.id === employeeId);
    if (employee == null) {
      return null;
    }

    return `${employee.employeeNo} - ${employee.lastName} ${employee.firstName}`;
  };

  private loadOfficialBusinessStatusList(): void {
    this.officialBusinessService.getStatusList().subscribe((data) => {
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
}
