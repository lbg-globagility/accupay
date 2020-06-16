import { Component, OnInit, Inject } from '@angular/core';
import { PayrollResultDetails } from '../shared/payroll-result-details';
import { PayrollResult } from '../shared/payroll-result';
import { Sort } from '@angular/material/sort';
import { PayPeriodService } from '../services/payperiod.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-payroll-result-details',
  templateUrl: './payroll-result-details.component.html',
  styleUrls: ['./payroll-result-details.component.scss'],
})
export class PayrollResultDetailsComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employeeNo',
    'employeeName',
    'description',
  ];

  filter: string = 'Success';

  result: PayrollResult;

  dataSource: PayrollResultDetails[];

  details: PayrollResultDetails[];

  constructor(
    private payPeriodService: PayPeriodService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.result = data.result;
  }

  ngOnInit(): void {
    this.dataSource = this.result.details.filter(
      (x) => x.status === this.filter
    );
  }

  filterButton(filter: string): void {
    this.filter = filter;
    this.dataSource = this.result.details.filter((x) => x.status === filter);
  }
}
