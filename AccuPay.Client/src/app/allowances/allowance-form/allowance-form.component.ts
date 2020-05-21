import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Allowance } from 'src/app/allowances/shared/allowance';
import { AllowanceService } from 'src/app/allowances/allowance.service';
import { AllowanceType } from 'src/app/allowances/shared/allowance-type';

@Component({
  selector: 'app-allowance-form',
  templateUrl: './allowance-form.component.html',
  styleUrls: ['./allowance-form.component.scss'],
})
export class AllowanceFormComponent implements OnInit {
  @Input()
  allowance: Allowance;

  @Output()
  save: EventEmitter<Allowance> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, [Validators.required]],
    allowanceTypeId: [null, Validators.required],
    frequency: [null, Validators.required],
    startDate: [null, [Validators.required]],
    endDate: [null],
    amount: [null, Validators.required],
  });

  employees: Employee[];
  allowanceTypes: string[];
  frequencyList: AllowanceType[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private allowanceService: AllowanceService
  ) {}

  ngOnInit(): void {
    this.loadAllowanceTypes();
    this.loadAllowanceStatusList();

    if (this.allowance != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.allowance);
    } else {
      this.loadEmployees();
    }
  }

  private loadAllowanceTypes(): void {
    this.allowanceService.getAllowanceTypes().subscribe((data) => {
      this.allowanceTypes = data;
    });
  }

  private loadAllowanceStatusList(): void {
    this.allowanceService.GetFrequencyList().subscribe((data) => {
      this.frequencyList = data;
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

    const allowance = this.form.value as Allowance;
    this.save.emit(allowance);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
