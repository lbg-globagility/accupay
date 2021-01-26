import { Component, OnInit } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { Sort } from '@angular/material/sort';
import { Employee } from '../shared/employee';
import { MatTableDataSource } from '@angular/material/table';
import { Subject, BehaviorSubject } from 'rxjs';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { LoadingState } from 'src/app/core/states/loading-state';
import { MatDialog } from '@angular/material/dialog';
import { SelectPayperiodDialogComponent } from 'src/app/payroll/components/select-payperiod-dialog/select-payperiod-dialog.component';
import { flatMap, filter } from 'rxjs/operators';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-time-entry',
  templateUrl: './time-entry.component.html',
  styleUrls: ['./time-entry.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class TimeEntryComponent implements OnInit {
  readonly displayedColumns = ['cutoff', 'status', 'actions'];

  latestPayPeriod: PayPeriod;

  dataSource: MatTableDataSource<PayPeriod>;

  payPeriods: PayPeriod[];

  selectedPayPeriod: PayPeriod;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  loadingState: LoadingState = new LoadingState();
  startingPayrollState: LoadingState = new LoadingState();

  sort: Sort = {
    active: 'cutoff',
    direction: '',
  };

  modelChanged: Subject<any>;
  employees: Employee[];

  pageIndex: number = 0;
  pageSize: number = 10;
  totalPages: number;
  totalCount: number;
  searchTerm: string;

  selectedRow: number;

  constructor(
    private payPeriodService: PayPeriodService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadingState.changeToLoading();
    this.startingPayrollState.changeToNothing();
    this.loadLatest();
    this.loadList();
  }

  loadLatest(): void {
    this.payPeriodService.getLatest().subscribe((payPeriod) => {
      this.latestPayPeriod = payPeriod;

      this.isLoading.next(true);
    });
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

          this.startingPayrollState.changeToLoading();

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

            this.refreshDataSource();
          }
          this.startingPayrollState.changeToSuccess();
        },
        (err) => {
          this.startingPayrollState.changeToNothing();
          this.errorHandler.badRequest(err, 'Failed to start pay period.');
        }
      );
  }

  private refreshDataSource(): void {
    this.dataSource.data = [...this.dataSource.data];
  }
}
