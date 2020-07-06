import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {
  EmploymentPolicy,
  EmploymentPolicyForm,
} from 'src/app/employment-policies/shared';

@Component({
  selector: 'app-employment-policy-form',
  templateUrl: './employment-policy-form.component.html',
  styleUrls: ['./employment-policy-form.component.scss'],
})
export class EmploymentPolicyFormComponent implements OnInit {
  @Input()
  employmentPolicy: EmploymentPolicy;

  form: FormGroup = this.fb.group({
    name: [, [Validators.required]],
    workDaysPerYear: [],
    gracePeriod: [],
    computeNightDiff: [false],
    computeNightDiffOT: [false],
    computeRestDay: [false],
    computeRestDayOT: [false],
    computeSpecialHoliday: [false],
    computeRegularHoliday: [false],
  });

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): EmploymentPolicy {
    const value = this.form.value as EmploymentPolicyForm;
    const employmentPolicy = Object.assign({}, value) as EmploymentPolicy;
    employmentPolicy.workDaysPerYear = value.workDaysPerYear ?? 0;
    employmentPolicy.gracePeriod = value.gracePeriod ?? 0;

    return employmentPolicy;
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    if (this.employmentPolicy) {
      this.form.patchValue(this.employmentPolicy);
    }
  }
}
