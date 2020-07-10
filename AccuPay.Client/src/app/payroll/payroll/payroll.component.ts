import { Component, OnInit, ViewChild } from '@angular/core';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { filter, flatMap, map } from 'rxjs/operators';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';
import { LoadingState } from 'src/app/core/states/loading-state';
import { SelectPayperiodDialogComponent } from '../components/select-payperiod-dialog/select-payperiod-dialog.component';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { findIndex } from 'lodash';
import { PayPeriodViewModel } from './payperiod-viewmodel';
import { PaginatedList } from 'src/app/core/shared/paginated-list';

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

  dataSource: MatTableDataSource<PayPeriodViewModel>;

  loadingState: LoadingState = new LoadingState();

  isStartingPayroll: boolean = false;
  isClosingPayroll: boolean = false;
  isReopeningPayroll: boolean = false;
  isUpdatingPayroll: boolean = false;

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
      .pipe(
        map((payPeriodList) => {
          let list = new PaginatedList<PayPeriodViewModel>();

          list.pageNumber = payPeriodList.pageNumber;
          list.totalCount = payPeriodList.totalCount;
          list.totalPages = payPeriodList.totalPages;

          list.items = payPeriodList.items.map((payPeriod) => {
            return new PayPeriodViewModel(payPeriod);
          });

          return list;
        })
      )
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
            this.dataSource.data.unshift(
              new PayPeriodViewModel(this.latestPayPeriod)
            );

            this.refreshDataSource();
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

    if (payPeriodId == this.latestPayPeriod.id) {
      this.isClosingPayroll = true;
    }
    let currentViewModel = this.getCurrentViewModel(payPeriodId);
    this.updateIsUpdatingStatus(currentViewModel, true);

    this.isUpdatingPayroll = true;
    this.payPeriodService
      .close(payPeriodId)
      .subscribe(
        () => {
          this.updateStatus(payPeriodId, 'Closed');
        },
        (err) =>
          this.errorHandler.badRequest(err, 'Failed to close pay period.')
      )
      .add(this.stopLoading(currentViewModel));
  }

  reopenPayroll(payPeriod: PayPeriod): void {
    let payPeriodId = payPeriod?.id;

    if (!payPeriod?.id) return;

    if (payPeriodId == this.latestPayPeriod.id) {
      this.isReopeningPayroll = true;
    }
    let currentViewModel = this.getCurrentViewModel(payPeriodId);
    this.updateIsUpdatingStatus(currentViewModel, true);

    this.isUpdatingPayroll = true;
    this.payPeriodService
      .reopen(payPeriodId)
      .subscribe(
        () => {
          this.updateStatus(payPeriodId, 'Open');
        },
        (err) =>
          this.errorHandler.badRequest(err, 'Failed to reopen pay period.')
      )
      .add(this.stopLoading(currentViewModel));
  }

  private stopLoading(currentViewModel: PayPeriodViewModel) {
    return () => {
      this.isClosingPayroll = false;
      this.isUpdatingPayroll = false;
      this.isReopeningPayroll = false;
      this.updateIsUpdatingStatus(currentViewModel, false);
    };
  }

  private updateIsUpdatingStatus(
    currentViewModel: PayPeriodViewModel,
    isUpdating: boolean
  ) {
    if (currentViewModel) {
      currentViewModel.isUpdating = isUpdating;
      this.refreshDataSource();
    }
  }

  private updateStatus(payPeriodId: number, status: string): void {
    let selectedIndex = this.getSelectedIndex(payPeriodId);

    if (selectedIndex >= 0) {
      this.dataSource.data[selectedIndex].payPeriod.status = status;
      this.refreshDataSource();
    }

    if (payPeriodId == this.latestPayPeriod.id) {
      this.latestPayPeriod.status = status;
    }
  }

  private refreshDataSource(): void {
    this.dataSource.data = [...this.dataSource.data];
  }

  private getSelectedIndex(payPeriodId: number): number {
    return findIndex(this.dataSource.data, function (vm: PayPeriodViewModel) {
      return vm.payPeriod.id == payPeriodId;
    });
  }

  private getCurrentViewModel(payPeriodId: number): PayPeriodViewModel {
    if (!payPeriodId) return null;

    let index = this.getSelectedIndex(payPeriodId);

    if (index >= 0) {
      return this.dataSource.data[index];
    }
    return null;
  }
}
