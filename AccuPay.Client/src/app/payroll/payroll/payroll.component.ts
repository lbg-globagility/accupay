import { Component, OnInit, ViewChild } from '@angular/core';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { filter, flatMap } from 'rxjs/operators';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';
import { LoadingState } from 'src/app/core/states/loading-state';
import { SelectPayperiodDialogComponent } from '../components/select-payperiod-dialog/select-payperiod-dialog.component';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { findIndex } from 'lodash';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class PayrollComponent implements OnInit {
  @ViewChild('table', { static: true }) table;

  latestPayPeriod: PayPeriod;

  readonly displayedColumns = ['cutoff', 'status', 'actions'];

  dataSource: MatTableDataSource<PayPeriod>;

  loadingState: LoadingState = new LoadingState();

  isStartingPayroll: boolean = false;

  pageIndex: number = 0;
  pageSize: number = 10;
  totalPages: number;
  totalCount: number;
  searchTerm: string;

  sort: Sort = {
    active: 'cutoff',
    direction: '',
  };

  constructor(
    private payPeriodService: PayPeriodService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadingState.changeToLoading();
    this.loadLatest();
    this.loadList();
  }

  loadLatest(): void {
    this.payPeriodService
      .getLatest()
      .subscribe((payPeriod) => (this.latestPayPeriod = payPeriod));
  }

  loadList(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.payPeriodService
      .getList(options, this.searchTerm)
      .subscribe((data) => {
        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(data.items);
        this.loadingState.changeToSuccess();
      });
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadList();
  }

  startNewPayroll(): void {
    this.dialog
      .open(SelectPayperiodDialogComponent, {
        minWidth: '500px',
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .pipe(
        flatMap((result) => {
          var payPeriod = result as PayPeriod;
          if (!payPeriod) return;

          this.isStartingPayroll = true;

          return this.payPeriodService.start(
            payPeriod.month,
            payPeriod.year,
            payPeriod.isFirstHalf
          );
        })
      )
      .subscribe(
        (payPeriod) => {
          this.latestPayPeriod = payPeriod;

          if (this.pageIndex == 0) {
            this.dataSource.data.pop();
            this.dataSource.data.unshift(this.latestPayPeriod);

            this.dataSource.data = [...this.dataSource.data];
          }
        },
        (err) =>
          this.errorHandler.badRequest(err, 'Failed to start pay period.')
      )
      .add(() => {
        this.isStartingPayroll = false;
      });
  }

  closePayroll(payPeriod: PayPeriod): void {
    let payPeriodId = payPeriod?.id;

    if (!payPeriod?.id) return;

    this.payPeriodService.close(payPeriodId).subscribe(
      () => {
        this.updateStatus(payPeriodId, 'Closed');
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to close pay period.')
    );
  }

  reopenPayroll(payPeriod: PayPeriod): void {
    let payPeriodId = payPeriod?.id;

    if (!payPeriod?.id) return;

    this.payPeriodService.reopen(payPeriodId).subscribe(
      () => {
        this.updateStatus(payPeriodId, 'Open');
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to reopen pay period.')
    );
  }

  private updateStatus(payPeriodId: number, status: string) {
    let selectedIndex = findIndex(this.dataSource.data, function (payPeriod) {
      return payPeriod.id == payPeriodId;
    });

    if (selectedIndex >= 0) {
      this.dataSource.data[selectedIndex].status = status;
      this.dataSource.data = [...this.dataSource.data];
    }

    if (payPeriodId == this.latestPayPeriod.id) {
      this.latestPayPeriod.status = status;
    }
  }
}
