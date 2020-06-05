import { Component, OnInit } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { TimeEntryService } from '../time-entry.service';
import { LoadingState } from 'src/app/core/states/loading-state';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Sort } from '@angular/material/sort';
import { auditTime } from 'rxjs/operators';
import { Employee } from '../shared/employee';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-time-entry',
  templateUrl: './time-entry.component.html',
  styleUrls: ['./time-entry.component.scss'],
})
export class TimeEntryComponent implements OnInit {
  readonly displayedColumns = ['cutoff', 'status', 'actions'];

  latestPayPeriod: PayPeriod;

  dataSource: MatTableDataSource<PayPeriod>;

  payPeriods: PayPeriod[];

  selectedPayPeriod: PayPeriod;

  savingState: LoadingState = new LoadingState();

  sort: Sort = {
    active: 'lastName',
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

  constructor(private payPeriodService: PayPeriodService) {
    // this.modelChanged = new Subject();
    // this.modelChanged
    //   .pipe(auditTime(Constants.ThrottleTime))
    //   .subscribe(() => this.load());
  }

  ngOnInit(): void {
    this.loadLatest();
    this.loadList();
  }

  loadLatest() {
    this.payPeriodService
      .getLatest()
      .subscribe((payPeriod) => (this.latestPayPeriod = payPeriod));
  }

  loadList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.payPeriodService
      .GetList(options, this.searchTerm)
      .subscribe((data) => {
        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(data.items);
      });
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number) {
    this.selectedRow = id;
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadList();
  }
}
