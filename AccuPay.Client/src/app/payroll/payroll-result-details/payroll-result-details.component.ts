import { Component, OnInit, Inject } from '@angular/core';
import { PayrollResultDetails } from '../shared/payroll-result-details';
import { PayrollResult } from '../shared/payroll-result';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-payroll-result-details',
  templateUrl: './payroll-result-details.component.html',
  styleUrls: ['./payroll-result-details.component.scss'],
})
export class PayrollResultDetailsComponent implements OnInit {
  displayedColumns: string[] = [];

  readonly displayedSuccessColumns: string[] = ['employeeNo', 'employeeName'];
  readonly displayedFailedColumns: string[] = [
    'employeeNo',
    'employeeName',
    'description',
  ];

  readonly successFilter: string = 'Success';
  readonly failedFilter: string = 'Error';
  filter: string = this.successFilter;

  result: PayrollResult;

  dataSource: PayrollResultDetails[];

  details: PayrollResultDetails[];

  constructor(
    private dialogRef: MatDialogRef<PayrollResultDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.result = data.result;
  }

  ngOnInit(): void {
    this.displayedColumns = this.displayedSuccessColumns;
    this.dataSource = this.result.details.filter(
      (x) => x.status === this.filter
    );
  }

  filterButton(filter: string): void {
    if (filter == this.successFilter) {
      this.displayedColumns = this.displayedSuccessColumns;
    } else {
      this.displayedColumns = this.displayedFailedColumns;
    }
    this.filter = filter;
    this.dataSource = this.result.details.filter((x) => x.status === filter);
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
