import { Component, OnInit } from '@angular/core';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { StartPayrollDialogComponent } from 'src/app/payroll/components/start-payroll-dialog/start-payroll-dialog.component';
import { filter, mergeMap } from 'rxjs/operators';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
})
export class PayrollComponent implements OnInit {
  latestPayperiod: PayPeriod;

  readonly displayedColumns = ['cutoff', 'status', 'actions'];

  dataSource: MatTableDataSource<PayPeriod>;

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
    private payperiodService: PayPeriodService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadLatest();
    this.loadList();
  }

  loadLatest(): void {
    this.payperiodService
      .getLatest()
      .subscribe((payperiod) => (this.latestPayperiod = payperiod));
  }

  loadList(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.payperiodService
      .GetList(options, this.searchTerm)
      .subscribe((data) => {
        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(data.items);
      });
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadList();
  }

  startPayroll(): void {
    this.dialog
      .open(StartPayrollDialogComponent)
      .afterClosed()
      .pipe(filter((t) => t != null))
      .pipe(
        mergeMap(({ cutoffStart, cutoffEnd }) =>
          this.payperiodService.start(cutoffStart, cutoffEnd)
        )
      )
      .subscribe({
        next: () => {},
      });
  }
}
