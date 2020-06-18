import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-start-payroll-dialog',
  templateUrl: './start-payroll-dialog.component.html',
  styleUrls: ['./start-payroll-dialog.component.scss'],
})
export class StartPayrollDialogComponent implements OnInit {
  form: FormGroup = this.fb.group({
    cutoffStart: [, [Validators.required]],
    cutoffEnd: [, [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<StartPayrollDialogComponent>
  ) {}

  ngOnInit(): void {}

  start() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    const value = this.form.value;
    this.dialogRef.close(value);
  }
}
