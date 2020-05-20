import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Salary } from 'src/app/salaries/shared/salary';

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
    basicSalary: [null, [Validators.required]],
    allowanceSalary: [null, Validators.required],
    effectiveFrom: [null, Validators.required],
    doPaySSSContribution: [false],
    autoComputePhilHealthContribution: [false],
    philHealthDeduction: [null],
    autoComputeHDMFContribution: [false],
    hdmfDeduction: [null],
  });

  computedTotalSalary: number;

  constructor(private fb: FormBuilder) {
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
      this.form.patchValue(this.salary);
    }
  }

  onSave(): void {
    console.log(this.form.get('effectiveFrom'));
    console.log(new Date(this.form.get('effectiveFrom').value));

    if (!this.form.valid) {
      return;
    }

    const salary = this.form.value as Salary;
    console.log(salary);
    // salary.effectiveFrom = new Date()

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
