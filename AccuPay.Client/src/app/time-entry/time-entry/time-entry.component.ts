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

  constructor(private payPeriodService: PayPeriodService) {}

  ngOnInit(): void {
    this.loadingState.changeToLoading();
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
      .GetList(options, this.searchTerm)
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
