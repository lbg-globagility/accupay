import { Component, OnInit, Inject } from '@angular/core';
import { LoanHistory } from '../shared/loan-history';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';
import { LoanService } from '../loan.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { PageOptions } from 'src/app/core/shared/page-options';

@Component({
  selector: 'app-loan-history',
  templateUrl: './loan-history.component.html',
  styleUrls: ['./loan-history.component.scss'],
})
export class LoanHistoryComponent implements OnInit {
  readonly displayedColumns: string[] = ['date', 'amount', 'balance'];

  employeeId: number;

  loanId: number;

  type: string;

  totalCount: number;

  dataSource: MatTableDataSource<LoanHistory>;

  pageIndex = 0;

  pageSize = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  constructor(
    private loanService: LoanService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.loanId = data.loanId;
  }

  ngOnInit(): void {
    this.getHistory();
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getHistory();
  }

  getHistory() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.loanService.getHistory(options, this.loanId).subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalCount = data.totalCount;
    });
  }
}
