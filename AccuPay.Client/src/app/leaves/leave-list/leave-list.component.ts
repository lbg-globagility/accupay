import { auditTime, filter } from 'rxjs/operators';
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
import { MatDialog } from '@angular/material/dialog';
import { NewLeaveComponent } from 'src/app/leaves/new-leave/new-leave.component';
import { EditLeaveComponent } from 'src/app/leaves/edit-leave/edit-leave.component';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

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
    'employee',
    'leaveType',
    'date',
    'time',
    'status',
    'actions',
  ];

  expandedLeave: Leave;

  searchTerm: string;

  modelChanged: Subject<any>;

  totalCount: number;

  dataSource: MatTableDataSource<Leave>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  constructor(
    private leaveService: LeaveService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadLeaves());
  }

  ngOnInit(): void {
    this.loadLeaves();
  }

  loadLeaves(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.leaveService.getAll(options, this.searchTerm).subscribe((data) => {
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(data.items);
    });
  }

  clearSearchBox(): void {
    this.searchTerm = '';
    this.applyFilter();
  }

  applyFilter(): void {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  sortData(sort: Sort): void {
    this.sort = sort;
    this.modelChanged.next();
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadLeaves();
  }

  toggleExpansion(leave: Leave) {
    if (this.expandedLeave === leave) {
      this.expandedLeave = null;
    } else {
      this.expandedLeave = leave;
    }
  }

  newLeave() {
    this.dialog
      .open(NewLeaveComponent)
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadLeaves());
  }

  editLeave(leave: Leave) {
    this.dialog
      .open(EditLeaveComponent, {
        data: {
          leaveId: leave.id,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadLeaves());
  }

  deleteLeave(leave: Leave): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Leave',
        content: 'Are you sure you want to delete this leave?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) {
        return;
      }

      this.leaveService.delete(leave.id).subscribe({
        next: () => {
          Swal.fire({
            title: 'Deleted',
            text: 'The leave was successfully deleted.',
            icon: 'success',
            showConfirmButton: true,
          });

          this.loadLeaves();
        },
        error: (err) =>
          this.errorHandler.badRequest(err, 'Failed to delete leave.'),
      });
    });
  }
}
