import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EmploymentPolicyService } from 'src/app/employment-policies/services/employment-policy.service';
import {
  EmploymentPolicyForm,
  EmploymentPolicy,
} from 'src/app/employment-policies/shared';

@Component({
  selector: 'app-new-employment-policy',
  templateUrl: './new-employment-policy.component.html',
  styleUrls: ['./new-employment-policy.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewEmploymentPolicyComponent implements OnInit {
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

  constructor(
    private employmentPolicyService: EmploymentPolicyService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {}

  save() {
    const form = this.form.value as EmploymentPolicyForm;

    const employmentPolicy: EmploymentPolicy = Object.assign(
      {},
      form
    ) as EmploymentPolicy;

    employmentPolicy.workDaysPerYear = form.workDaysPerYear ?? 0;
    employmentPolicy.gracePeriod = form.workDaysPerYear ?? 0;

    this.employmentPolicyService.create(employmentPolicy).subscribe();
  }

  cancel(): void {
    this.router.navigate(['employment-policies']);
  }
}
