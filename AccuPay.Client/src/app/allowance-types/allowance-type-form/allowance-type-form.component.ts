import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AllowanceType } from 'src/app/allowances/shared/allowance-type';
import { cloneDeep } from 'lodash';

@Component({
  selector: 'app-allowance-type-form',
  templateUrl: './allowance-type-form.component.html',
  styleUrls: ['./allowance-type-form.component.scss'],
})
export class AllowanceTypeFormComponent implements OnInit {
  @Input()
  allowanceType: AllowanceType;

  frequencyList = ['Daily', 'Weekly', 'Semi-Monthly', 'Monthly'];

  form: FormGroup = this.fb.group({
    id: [null],
    name: [null, [Validators.required]],
    displayString: [null, [Validators.required]],
    frequency: [null, Validators.required],
    isTaxable: [null],
    is13thMonthPay: [null],
    isFixed: [null],
  });

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form.patchValue(this.allowanceType);
  }

  get value(): AllowanceType {
    const allowanceType = cloneDeep(this.form.value as AllowanceType);

    return allowanceType;
  }
}
