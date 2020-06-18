import { Component, OnInit, Inject } from '@angular/core';
import { PayrollResultDetails } from '../../shared/payroll-result-details';
import { PayrollResult } from '../../shared/payroll-result';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-payroll-result-details',
  templateUrl: './payroll-result-details.component.html',
  styleUrls: ['./payroll-result-details.component.scss'],
})
export class PayrollResultDetailsComponent implements OnInit {
  readonly displayedSuccessColumns: string[] = ['employee'];

  readonly displayedErrorColumns: string[] = ['employee', 'description'];

  result: PayrollResult;

  successDataSource: PayrollResultDetails[];

  errorDataSource: PayrollResultDetails[];

  constructor(
    private dialogRef: MatDialogRef<PayrollResultDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.result = data.result;
  }

  ngOnInit(): void {
    this.successDataSource = this.result.details.filter(
      (x) => x.status === 'Success'
    );
    this.errorDataSource = this.result.details.filter(
      (x) => x.status === 'Error'
    );
  }
}
