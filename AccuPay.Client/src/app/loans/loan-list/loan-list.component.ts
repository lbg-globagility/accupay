import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanService } from 'src/app/loans/loan.service';

@Component({
  selector: 'app-loan-list',
  templateUrl: './loan-list.component.html',
  styleUrls: ['./loan-list.component.scss'],
})
export class LoanListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employeeNumber',
    'employeeName',
    'loanType',
    'deductionSchedule',
    'startDate',
    'totalLoanAmount',
    'totalBalanceLeft',
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  loans: Loan[];

  totalCount: number;

  dataSource: MatTableDataSource<Loan>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'startDate',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private loanService: LoanService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getLoanList());
  }

  ngOnInit(): void {
    this.getLoanList();
  }

  getLoanList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.loanService.getAll(options, this.searchTerm).subscribe((data) => {
      this.loans = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.loans);
    });
  }

  applyFilter(searchTerm: string) {
    this.searchTerm = searchTerm;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
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
    this.getLoanList();
  }
}
