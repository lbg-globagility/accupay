import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent {
  form: FormGroup = this.fb.group({
    email: [, [Validators.required]],
    firstName: [, [Validators.required]],
    lastName: [, [Validators.required]],
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
