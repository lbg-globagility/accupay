import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Injectable } from '@angular/core';
import { Organization } from 'src/app/accounts/shared/organization';

@Injectable()
export class OrganizationFormService {
  form: FormGroup = this.fb.group({
    id: [],
    address1: [],
    address2: [],
    city: [],
    state: [],
    image: [],
    zip: [],
    domains: this.fb.array([]),
    acknowledgmentThresholds: this.fb.group({
      items: this.fb.array([
        this.fb.group({
          classification: ['Class I'],
          first: [],
          second: [, Validators.required]
        }),
        this.fb.group({
          classification: ['Class II'],
          first: [],
          second: [, Validators.required]
        }),
        this.fb.group({
          classification: ['Class III'],
          first: [],
          second: [, Validators.required]
        })
      ])
    }),
    inProgressThresholds: this.fb.group({
      items: this.fb.array([
        this.fb.group({
          classification: ['Class I'],
          first: [],
          second: [, Validators.required]
        }),
        this.fb.group({
          classification: ['Class II'],
          first: [],
          second: [, Validators.required]
        }),
        this.fb.group({
          classification: ['Class III'],
          first: [],
          second: [, Validators.required]
        })
      ])
    })
  });

  constructor(private fb: FormBuilder) {}

  get value() {
    const value = this.form.value;

    value.acknowledgmentThresholds = value.acknowledgmentThresholds.items;
    value.inProgressThresholds = value.inProgressThresholds.items;

    return value;
  }

  get acknowledgmentThresholdArray(): FormArray {
    return this.form.get('acknowledgmentThresholds').get('items') as FormArray;
  }

  get inProgressThresholdArray(): FormArray {
    return this.form.get('inProgressThresholds').get('items') as FormArray;
  }

  patchInfo(organization: Organization) {
    this.valuePatcher('address1', organization.address1);
    this.valuePatcher('address2', organization.address2);
    this.valuePatcher('city', organization.city);
    this.valuePatcher('state', organization.state);
    this.valuePatcher('zip', organization.zip);
  }

  private valuePatcher(formName: string, assignValue: any) {
    this.form.get(formName).patchValue(assignValue);
  }

  patchValue(organization: Organization) {
    this.acknowledgmentThresholdArray.patchValue(
      organization.acknowledgmentThresholds
    );

    this.inProgressThresholdArray.patchValue(organization.inProgressThresholds);
  }
}
