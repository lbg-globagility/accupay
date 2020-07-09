import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Employee } from '../shared/employee';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { EmploymentPolicyService } from 'src/app/employment-policies/services/employment-policy.service';
import { EmploymentPolicy } from 'src/app/employment-policies/shared';
import { PositionService } from 'src/app/positions/position.service';
import { Position } from 'src/app/positions/shared/position';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss'],
})
export class EmployeeFormComponent implements OnInit {
  @Input()
  employee: Employee;

  @Input()
  uncontrollable = false;

  @Output()
  save: EventEmitter<Employee> = new EventEmitter();

  @Output()
  cancel = new EventEmitter<void>();

  form: FormGroup = this.fb.group({
    employeeNo: [null, Validators.required],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    middleName: [],
    birthdate: [, Validators.required],
    address: [],
    landlineNo: [],
    mobileNo: [],
    emailAddress: [],
    tin: [],
    sssNo: [],
    philHealthNo: [],
    pagIbigNo: [],
    employmentStatus: [, Validators.required],
    employmentPolicyId: [],
    startDate: [, [Validators.required]],
    regularizationDate: [],
    positionId: [],
  });

  employmentStatuses: string[] = [];

  employmentPolicies: EmploymentPolicy[] = [];

  positions: Position[];

  constructor(
    private employeeService: EmployeeService,
    private employmentPolicyService: EmploymentPolicyService,
    private positionService: PositionService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loadEmploymentStatuses();
    this.loadEmploymentPolicies();
    this.loadPositions();
    this.initForm();
  }

  initForm() {
    if (this.employee != null) {
      this.form.patchValue(this.employee);

      this.form.patchValue({
        positionId: this.employee.position?.id,
      });
    }
  }

  onSave() {
    if (this.form.valid) {
      this.save.emit(this.form.value);
    }
  }

  getValue(property: string) {
    return this.form.get(property).value;
  }

  private loadPositions(): void {
    this.positionService
      .getAll()
      .subscribe((positions) => (this.positions = positions));
  }

  private loadEmploymentStatuses(): void {
    this.employeeService
      .getEmploymentStatuses()
      .subscribe(
        (employmentStatuses) => (this.employmentStatuses = employmentStatuses)
      );
  }

  private loadEmploymentPolicies(): void {
    this.employmentPolicyService
      .getAll()
      .subscribe(
        (employmentPolicies) => (this.employmentPolicies = employmentPolicies)
      );
  }
}
