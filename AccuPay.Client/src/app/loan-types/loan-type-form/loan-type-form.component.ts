import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoanType } from 'src/app/loan-types/shared/loan-type';
import { cloneDeep } from 'lodash';

@Component({
  selector: 'app-loan-type-form',
  templateUrl: './loan-type-form.component.html',
  styleUrls: ['./loan-type-form.component.scss'],
})
export class LoanTypeFormComponent implements OnInit {
  @Input()
  loanType: LoanType;

  form: FormGroup = this.fb.group({
    id: [null],
    name: [null, Validators.required],
  });

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form.patchValue(this.loanType);
  }

  get value(): LoanType {
    const loanType = cloneDeep(this.form.value as LoanType);

    return loanType;
  }
}
