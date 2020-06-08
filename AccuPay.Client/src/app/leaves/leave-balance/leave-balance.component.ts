import { Component, OnInit } from '@angular/core';
import { Employee } from 'src/app/employees/shared/employee';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { Sort } from '@angular/material/sort';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { auditTime, filter } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { LeaveLedgerComponent } from 'src/app/leaves/leave-ledger/leave-ledger.component';
import { LeaveService } from '../leave.service';
import { LeaveBalance } from '../shared/leave-balance';

@Component({
  selector: 'app-leave-balance',
  templateUrl: './leave-balance.component.html',
  styleUrls: ['./leave-balance.component.scss']
})
export class LeaveBalanceComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employee',
    'vacationLeaveBalance',
    'sickLeaveBalance',
  ];

  totalPages: number;

  totalCount: number;

  term: string;

  placeholder: string;

  dataSource: MatTableDataSource<LeaveBalance>;

  modelChanged: Subject<any>;

  pageIndex = 0;

  pageSize = 10;

  sort: Sort = {
    active: 'employee',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(
    private leaveService: LeaveService,
    private employeeService: EmployeeService,
    private dialog: MatDialog,
    ){

    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.load());
  }

  ngOnInit(): void {
    this.load();
  }

  load() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.leaveService.getBalance(options, this.term).subscribe(async (data) => {
      setTimeout(() => {
        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(data.items);
      });
    });
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.load();
  }

  clearSearchBox() {
    this.term = '';
    this.applyFilter();
  }

  applyFilter() {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number) {
    this.selectedRow = id;
  }

  showLedger(leaveBalance: LeaveBalance, type: string) {
    this.dialog
      .open(LeaveLedgerComponent, {
        data: {
          employeeId: leaveBalance.employeeId,
          type: type,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.load());
  }
}
