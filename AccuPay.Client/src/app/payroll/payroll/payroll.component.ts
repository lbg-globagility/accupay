import { Component, OnInit, ViewChild } from '@angular/core';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { MatTableDataSource } from '@angular/material/table';
import { map } from 'rxjs/operators';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';
import { LoadingState } from 'src/app/core/states/loading-state';
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

  dataSource: MatTableDataSource<PayPeriod>;

  loadingState: LoadingState = new LoadingState();

  pageIndex: number = 0;
  pageSize: number = 10;
  totalPages: number;
  totalCount: number;
  searchTerm: string;

  sort: Sort = {
    active: 'cutoff',
    direction: '',
  };

  constructor(private payPeriodService: PayPeriodService) {}

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
}
