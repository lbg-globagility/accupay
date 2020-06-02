import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';
import {
  state,
  trigger,
  transition,
  animate,
  style,
} from '@angular/animations';

@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class LeaveListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employeeNumber',
    // 'employeeName',
    'leaveType',
    'date',
    'time',
    'status',
    'actions',
  ];

  expandedLeave: Leave;

  searchTerm: string;

  modelChanged: Subject<any>;

  leaves: Leave[];

  totalCount: number;

  dataSource: MatTableDataSource<Leave>;

  clearSearch = '';

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  selectedRow: number;

  constructor(private leaveService: LeaveService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getLeaveList());
  }

  ngOnInit(): void {
    this.getLeaveList();
  }

  getLeaveList(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.leaveService.getAll(options, this.searchTerm).subscribe((data) => {
      this.leaves = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.leaves);
    });
  }

  clearSearchBox(): void {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
  }

  applyFilter(searchTerm: string): void {
    this.searchTerm = searchTerm;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  sortData(sort: Sort): void {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number): void {
    this.selectedRow = id;
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getLeaveList();
  }

  toggleExpansion(leave: Leave) {
    if (this.expandedLeave === leave) {
      this.expandedLeave = null;
    } else {
      this.expandedLeave = leave;
    }
  }
}
