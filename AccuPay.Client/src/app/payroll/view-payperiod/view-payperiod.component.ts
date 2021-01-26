import { Component, OnInit, ViewChild } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { ActivatedRoute, Router } from '@angular/router';
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
import { MatPaginator } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { ReportService } from 'src/app/reports/report.service';
import { LoadingState } from 'src/app/core/states/loading-state';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { PermissionTypes } from 'src/app/core/auth';

@Component({
  selector: 'app-view-payperiod',
  templateUrl: './view-payperiod.component.html',
  styleUrls: ['./view-payperiod.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewPayPeriodComponent implements OnInit {
  readonly PermissionTypes = PermissionTypes;

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  private payPeriodId: number = +this.route.snapshot.paramMap.get('id');

  payPeriod: PayPeriod;

  readonly displayedColumns = ['employee', 'netPay'];

  dataSource: MatTableDataSource<Paystub>;

  sort: Sort = {
    active: 'employee',
    direction: '',
  };

  pageIndex: number = 0;

  pageSize: number = 10;

  searchTerm: string;

  expandedPaystub: Paystub;

  loadingState: LoadingState = new LoadingState();

  updatingState: LoadingState = new LoadingState();

  constructor(
    private payPeriodService: PayPeriodService,
    private reportService: ReportService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadingState.changeToLoading();
    this.updatingState.changeToNothing();

    this.loadPayPeriod();
    this.loadPaystubs();
  }

  searchChanged(): void {
    this.dataSource.filter = this.searchTerm;
  }

  toggleExpansion(paystub: Paystub): void {
    this.expandedPaystub = paystub;
  }

  calculate(): void {
    this.snackBar.open('Calculating payroll');

    this.updatingState.changeToLoading();
    this.payPeriodService.calculate(this.payPeriodId).subscribe({
      next: (result) => {
        this.loadPaystubs();
        this.showDetails(result);
        this.snackBar.dismiss();
        this.updatingState.changeToSuccess();
      },
      error: (err) => {
        this.updatingState.changeToNothing();
        this.errorHandler.badRequest(err, 'Failed to calculate payroll');
      },
    });
  }

  showDetails(result: PayrollResult): void {
    this.dialog.open(PayrollResultDetailsComponent, {
      data: { result },
    });
  }

  downloadPayslips(): void {
    this.snackBar.open('Downloading Payslips...');

    this.payPeriodService
      .getPayslipPDF(this.payPeriodId)
      .then(() => this.snackBar.dismiss())
      .catch((err) =>
        this.errorHandler.badRequest(err, 'Error downloading payslips')
      );
  }

  downloadSummary(): void {
    this.snackBar.open('Downloading Summary...');

    this.reportService
      .getPayrollSummary(this.payPeriodId, this.payPeriodId)
      .then(() => this.snackBar.dismiss())
      .catch((err) =>
        this.errorHandler.badRequest(err, 'Error downloading payroll summary')
      );
  }

  closePayroll(): void {
    let payPeriodId = this.payPeriod?.id;

    if (!this.payPeriod?.id) return;

    let closeFunction = () => {
      this.snackBar.open('Closing Payroll...');
      this.updatingState.changeToLoading();
      this.payPeriodService.close(payPeriodId).subscribe(
        () => {
          this.payPeriod.status = 'Closed';
          this.snackBar.dismiss();
          this.updatingState.changeToSuccess();
        },
        (err) => {
          this.updatingState.changeToNothing();
          this.errorHandler.badRequest(err, 'Failed to close pay period.');
        }
      );
    };

    this.confirmAction(
      {
        title: 'Close Payroll',
        content: 'Are you sure you want to close this payroll?',
        confirmButtonText: 'Close Payroll',
        confirmButtonColor: 'primary',
      },
      closeFunction
    );
  }

  reopenPayroll(): void {
    let payPeriodId = this.payPeriod?.id;

    if (!this.payPeriod?.id) return;

    let reopenFunction = () => {
      this.updatingState.changeToLoading();
      this.snackBar.open('Reopening Payroll...');
      this.payPeriodService.reopen(payPeriodId).subscribe(
        () => {
          this.payPeriod.status = 'Open';
          this.snackBar.dismiss();
          this.updatingState.changeToSuccess();
        },
        (err) => {
          this.updatingState.changeToNothing();
          this.errorHandler.badRequest(err, 'Failed to reopen pay period.');
        }
      );
    };

    this.confirmAction(
      {
        title: 'Reopen Payroll',
        content: 'Are you sure you want to reopen this payroll?',
        confirmButtonText: 'Reopen Payroll',
        confirmButtonColor: 'primary',
      },
      reopenFunction
    );
  }

  deletePayroll(): void {
    let payPeriodId = this.payPeriod?.id;

    if (!this.payPeriod?.id) return;

    let deleteFunction = () => {
      this.updatingState.changeToLoading();
      this.snackBar.open('Deleting Payroll...');
      this.payPeriodService.delete(payPeriodId).subscribe(
        () => {
          window.location.reload();
        },
        (err) => {
          this.updatingState.changeToNothing();
          this.errorHandler.badRequest(err, 'Failed to delete pay period.');
        }
      );
    };

    this.confirmAction(
      {
        title: 'Delete Payroll',
        content: 'Are you sure you want to delete this payroll?',
        confirmButtonText: 'Delete Payroll',
        confirmButtonColor: 'primary',
      },
      deleteFunction
    );
  }

  cancelPayroll(): void {
    let payPeriodId = this.payPeriod?.id;

    if (!this.payPeriod?.id) return;

    let cancelFunction = () => {
      this.updatingState.changeToLoading();
      this.snackBar.open('Cancelling Payroll...');
      this.payPeriodService.cancel(payPeriodId).subscribe(
        () => {
          this.snackBar.dismiss();
          this.router.navigate(['payroll']);
        },
        (err) => {
          this.updatingState.changeToNothing();
          this.errorHandler.badRequest(err, 'Failed to cancel pay period.');
        }
      );
    };

    this.confirmAction(
      {
        title: 'Cancel Payroll',
        content: 'Are you sure you want to cancel this payroll?',
        confirmButtonText: 'Cancel Payroll',
        confirmButtonColor: 'primary',
      },
      cancelFunction
    );
  }

  private loadPayPeriod(): void {
    this.payPeriodService
      .getById(this.payPeriodId)
      .subscribe((payPeriod) => (this.payPeriod = payPeriod));
  }

  private loadPaystubs(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.payPeriodService
      .getPaystubs(this.payPeriodId, options, this.searchTerm)
      .subscribe((data) => {
        this.loadingState.changeToSuccess();

        this.dataSource = new MatTableDataSource(data);
        this.dataSource.paginator = this.paginator;
        this.expandedPaystub = data[0];
      });
  }

  private confirmAction(data: any, action: Function): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: data,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      action();
    });
  }
}
