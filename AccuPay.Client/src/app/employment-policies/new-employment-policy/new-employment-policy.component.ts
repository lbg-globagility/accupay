import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EmploymentPolicy } from 'src/app/employment-policies/shared/employment-policy';

@Component({
  selector: 'app-new-employment-policy',
  templateUrl: './new-employment-policy.component.html',
  styleUrls: ['./new-employment-policy.component.scss'],
})
export class NewEmploymentPolicyComponent implements OnInit {
  form: FormGroup = this.fb.group({
    name: [, [Validators.required]],
    workDaysPerYear: [],
    gracePeriod: [],
  });

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {}

  save() {
    const value = this.form.value;

    console.log(value);
  }
}
