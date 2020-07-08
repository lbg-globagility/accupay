import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-organization-form',
  templateUrl: './organization-form.component.html',
  styleUrls: ['./organization-form.component.scss'],
})
export class OrganizationFormComponent {
  form: FormGroup = this.fb.group({
    name: [, [Validators.required]],
  });

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): any {
    return this.form.value;
  }

  constructor(private fb: FormBuilder) {}
}
