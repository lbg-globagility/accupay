import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-employment-policy-form',
  templateUrl: './employment-policy-form.component.html',
  styleUrls: ['./employment-policy-form.component.scss'],
})
export class EmploymentPolicyFormComponent implements OnInit {
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

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {}
}
