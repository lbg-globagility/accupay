import { Component, OnInit } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { ActivatedRoute } from '@angular/router';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { MatTableDataSource } from '@angular/material/table';
import { Paystub } from 'src/app/payroll/shared/paystub';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { PayrollResult } from '../shared/payroll-result';
import { MatDialog } from '@angular/material/dialog';
import { PayrollResultDetailsComponent } from '../payroll-result-details/payroll-result-details.component';

@Component({
  selector: 'app-view-payperiod',
  templateUrl: './view-payperiod.component.html',
  styleUrls: ['./view-payperiod.component.scss'],
})
export class ViewPayPeriodComponent implements OnInit {
  private payPeriodId: number = +this.route.snapshot.paramMap.get('id');

  payPeriod: PayPeriod;

  paystubs: Paystub[];

  payrollResult: PayrollResult;

  readonly displayedColumns = ['employee', 'netPay'];

  dataSource: MatTableDataSource<any>;

  constructor(
    private payPeriodService: PayPeriodService,
    private route: ActivatedRoute,
    private snackbar: MatSnackBar,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadPayPeriod();
    this.loadPaystubs();
  }

  loadPayPeriod() {
    this.payPeriodService
      .getById(this.payPeriodId)
      .subscribe((payPeriod) => (this.payPeriod = payPeriod));
  }

  loadPaystubs() {
    this.payPeriodService
      .getPaystubs(this.payPeriodId)
      .subscribe((paystubs) => (this.paystubs = paystubs));
  }

  showDetails() {
    this.dialog.open(PayrollResultDetailsComponent, {
      data: {
        result: this.payrollResult,
      },
    });
  }

  calculate() {
    this.snackbar.open('Calculating payroll');

    this.payPeriodService.calculate(this.payPeriodId).subscribe({
      next: (data) => {
        this.payrollResult = data;
        this.snackbar.open('Finished calculating payroll.', 'OK');
        this.loadPaystubs();
        this.showDetails();
      },
      error: (err) =>
        this.errorHandler.badRequest(err, 'Failed to calculate payroll'),
    });
  }
}
