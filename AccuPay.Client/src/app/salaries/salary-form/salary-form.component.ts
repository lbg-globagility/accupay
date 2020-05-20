import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Salary } from 'src/app/salaries/shared/salary';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Employee } from 'src/app/employees/shared/employee';

@Component({
  selector: 'app-salary-form',
  templateUrl: './salary-form.component.html',
  styleUrls: ['./salary-form.component.scss'],
})
export class SalaryFormComponent implements OnInit {
  @Input()
  salary: Salary;

  @Output()
  save: EventEmitter<Salary> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, [Validators.required]],
    basicSalary: [null, [Validators.required]],
    allowanceSalary: [null, Validators.required],
    effectiveFrom: [null, Validators.required],
    doPaySSSContribution: [true],
    autoComputePhilHealthContribution: [true],
    philHealthDeduction: [null, [Validators.required]],
    autoComputeHDMFContribution: [true],
    hdmfDeduction: [null, [Validators.required]],
  });

  computedTotalSalary: number;

  employees: Employee[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService
  ) {
    this.form
      .get('basicSalary')
      .valueChanges.subscribe(this.recomputeTotalSalary());
    this.form
      .get('allowanceSalary')
      .valueChanges.subscribe(this.recomputeTotalSalary());

    this.form
      .get('autoComputePhilHealthContribution')
      .valueChanges.subscribe(
        this.toggleContributionInput('philHealthDeduction')
      );
    this.form
      .get('autoComputeHDMFContribution')
      .valueChanges.subscribe(this.toggleContributionInput('hdmfDeduction'));
  }

  ngOnInit(): void {
    if (this.salary != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.salary);
    } else {
      this.form.get('philHealthDeduction').disable();
      this.form.get('hdmfDeduction').disable();
      this.loadEmployees();
    }
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

    const salary = this.form.value as Salary;
    this.save.emit(salary);
  }

  onCancel(): void {
    this.cancel.emit();
  }

  private toggleContributionInput(controlName: string): (value: any) => void {
    return (autoComputePhilHealthContribution: boolean) => {
      if (autoComputePhilHealthContribution) {
        this.form.get(controlName).disable();
      } else {
        this.form.get(controlName).enable();
      }
    };
  }

  private recomputeTotalSalary(): (value: any) => void {
    return () => {
      this.computedTotalSalary =
        this.form.get('basicSalary').value +
        this.form.get('allowanceSalary').value;
    };
  }
}
