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
import { PayrollResultDetailsComponent } from '../components/payroll-result-details/payroll-result-details.component';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';

@Component({
  selector: 'app-view-payperiod',
  templateUrl: './view-payperiod.component.html',
  styleUrls: ['./view-payperiod.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewPayPeriodComponent implements OnInit {
  private payPeriodId: number = +this.route.snapshot.paramMap.get('id');

  payPeriod: PayPeriod;

  payrollResult: PayrollResult;

  readonly displayedColumns = ['employee', 'grossPay', 'netPay'];

  dataSource: MatTableDataSource<Paystub>;

  totalCount: number;

  sort: Sort = {
    active: 'employee',
    direction: '',
  };

  pageIndex: number = 0;
  pageSize: number = 10;

  searchTerm: string;

  modelChanged: Subject<any>;

  expandedPaystub: Paystub;

  constructor(
    private payPeriodService: PayPeriodService,
    private route: ActivatedRoute,
    private snackbar: MatSnackBar,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadPaystubs());
  }

  ngOnInit(): void {
    this.loadPayPeriod();
    this.loadPaystubs();
  }

  loadPayPeriod(): void {
    this.payPeriodService
      .getById(this.payPeriodId)
      .subscribe((payPeriod) => (this.payPeriod = payPeriod));
  }

  loadPaystubs(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.payPeriodService
      .getPaystubs(this.payPeriodId, options, this.searchTerm)
      .subscribe((data) => {
        this.dataSource = new MatTableDataSource(data.items);
        this.totalCount = data.totalCount;
      });
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.modelChanged.next();
  }

  toggleExpansion(paystub: Paystub): void {
    this.expandedPaystub = this.expandedPaystub === paystub ? null : paystub;
  }

  calculate(): void {
    this.snackbar.open('Calculating payroll');

    this.payPeriodService.calculate(this.payPeriodId).subscribe({
      next: (data) => {
        this.payrollResult = data;
        this.loadPaystubs();
        this.showDetails();
        this.snackbar.dismiss();
      },
      error: (err) =>
        this.errorHandler.badRequest(err, 'Failed to calculate payroll'),
    });
  }

  showDetails(): void {
    this.dialog.open(PayrollResultDetailsComponent, {
      data: {
        result: this.payrollResult,
      },
    });
  }

  downloadFile(): void {
    this.payPeriodService.getDocument(this.payPeriodId);
  }
}
