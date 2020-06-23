import { auditTime, filter } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { MatDialog } from '@angular/material/dialog';
import { NewOvertimeComponent } from 'src/app/overtimes/new-overtime/new-overtime.component';
import { EditOvertimeComponent } from 'src/app/overtimes/edit-overtime/edit-overtime.component';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { Moment } from 'moment';

@Component({
  selector: 'app-overtime-list',
  templateUrl: './overtime-list.component.html',
  styleUrls: ['./overtime-list.component.scss'],
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
  host: {
    class: 'block h-full p-4',
  },
})
export class OvertimeListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employee',
    'date',
    'time',
    'status',
    'actions',
  ];

  expandedOvertime: Overtime;

  searchTerm: string;

  dateFrom: Moment;

  dateTo: Moment;

  modelChanged: Subject<any>;

  totalCount: number;

  dataSource: MatTableDataSource<Overtime>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  constructor(
    private overtimeService: OvertimeService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadOvertimes());
  }

  ngOnInit(): void {
    this.loadOvertimes();
  }

  loadOvertimes(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.overtimeService
      .getAll(options, this.searchTerm, this.dateFrom, this.dateTo)
      .subscribe((data) => {
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
    this.loadOvertimes();
  }

  toggleExpansion(overtime: Overtime) {
    if (this.expandedOvertime === overtime) {
      this.expandedOvertime = null;
    } else {
      this.expandedOvertime = overtime;
    }
  }

  newOvertime() {
    this.dialog
      .open(NewOvertimeComponent)
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadOvertimes());
  }

  editOvertime(overtime: Overtime) {
    this.dialog
      .open(EditOvertimeComponent, {
        data: {
          overtimeId: overtime.id,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadOvertimes());
  }

  deleteOvertime(overtime: Overtime): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Overtime',
        content: 'Are you sure you want to delete this overtime?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) {
        return;
      }

      this.overtimeService.delete(overtime.id).subscribe({
        next: () => {
          Swal.fire({
            title: 'Deleted',
            text: 'The overtime was successfully deleted.',
            icon: 'success',
            showConfirmButton: true,
          });

          this.loadOvertimes();
        },
        error: (err) =>
          this.errorHandler.badRequest(err, 'Failed to delete overtime.'),
      });
    });
  }
}
