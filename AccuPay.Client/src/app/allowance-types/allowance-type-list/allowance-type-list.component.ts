import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Component, OnInit } from '@angular/core';
import { AllowanceType } from 'src/app/allowances/shared/allowance-type';
import { Subject } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';
import { AllowanceTypeService } from '../service/allowance-type.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { auditTime, filter } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { EditAllowanceTypeComponent } from '../edit-allowance-type/edit-allowance-type.component';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';
import { NewAllowanceTypeComponent } from '../new-allowance-type/new-allowance-type.component';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';

@Component({
  selector: 'app-allowance-type-list',
  templateUrl: './allowance-type-list.component.html',
  styleUrls: ['./allowance-type-list.component.scss'],
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
export class AllowanceTypeListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'name',
    'displayString',
    'frequency',
    'actions',
  ];

  expandedAllowanceType: AllowanceType;

  searchTerm: string;

  modelChanged: Subject<any>;

  totalCount: number;

  dataSource: MatTableDataSource<AllowanceType>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'name',
    direction: '',
  };

  constructor(
    private service: AllowanceTypeService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getAllowanceTypes());
  }

  ngOnInit(): void {
    this.getAllowanceTypes();
  }

  getAllowanceTypes() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.service.getList(options, this.searchTerm).subscribe((data) => {
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(data.items);
    });
  }

  applyFilter() {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.searchTerm = '';
    this.applyFilter();
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getAllowanceTypes();
  }

  toggleExpansion(allowanceType: AllowanceType) {
    if (this.expandedAllowanceType === allowanceType) {
      this.expandedAllowanceType = null;
    } else {
      this.expandedAllowanceType = allowanceType;
    }
  }

  editAllowanceType(allowanceType: AllowanceType) {
    this.dialog
      .open(EditAllowanceTypeComponent, {
        data: {
          allowanceTypeId: allowanceType.id,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.getAllowanceTypes());
  }

  deleteAllowanceType(allowanceType: AllowanceType) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Allowance Type',
        content: 'Are you sure you want to delete this allowance type?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) {
        return;
      }

      this.service.delete(allowanceType.id).subscribe({
        next: () => {
          Swal.fire({
            title: 'Deleted',
            text: 'The allowance type was successfully deleted.',
            icon: 'success',
            showConfirmButton: true,
          });

          this.getAllowanceTypes();
        },
        error: (err) =>
          this.errorHandler.badRequest(
            err,
            'Failed to delete official business.'
          ),
      });
    });
  }

  newAllowanceType() {
    this.dialog
      .open(NewAllowanceTypeComponent)
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.getAllowanceTypes());
  }
}
