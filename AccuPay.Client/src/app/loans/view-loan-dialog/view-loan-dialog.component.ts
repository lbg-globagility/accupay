import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Loan } from 'src/app/loans/shared/loan';
import { PageEvent } from '@angular/material/paginator';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { LoanHistory } from 'src/app/loans/shared/loan-history';
import { LoanService } from 'src/app/loans/loan.service';

@Component({
  selector: 'app-view-loan-dialog',
  templateUrl: './view-loan-dialog.component.html',
  styleUrls: ['./view-loan-dialog.component.scss'],
})
export class ViewLoanDialogComponent implements OnInit {
  loan: Loan;

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
    private dialog: MatDialogRef<ViewLoanDialogComponent>,
    private loanService: LoanService,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.loan = data.loan;
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

    this.loanService.getHistory(options, this.loan.id).subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalCount = data.totalCount;
    });
  }
}
