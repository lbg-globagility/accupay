import { Component, OnInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { LoanType } from '../shared/loan-type';
import { Subject } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';
import { LoanTypeService } from '../service/loan-type.service';
import { MatDialog } from '@angular/material/dialog';
import { auditTime, filter } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { EditLoanTypeComponent } from '../edit-loan-type/edit-loan-type.component';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';
import { NewLoanTypeComponent } from '../new-loan-type/new-loan-type.component';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-loan-type-list',
  templateUrl: './loan-type-list.component.html',
  styleUrls: ['./loan-type-list.component.scss'],
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
export class LoanTypeListComponent implements OnInit {
  readonly displayedColumns: string[] = ['name', 'actions'];

  expandedLoanType: LoanType;

  searchTerm: string;

  modelChanged: Subject<any>;

  totalCount: number;

  dataSource: MatTableDataSource<LoanType>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'name',
    direction: '',
  };

  constructor(
    private service: LoanTypeService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getLoanTypes());
  }

  ngOnInit(): void {
    this.getLoanTypes();
  }

  getLoanTypes() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.service.list(options, this.searchTerm).subscribe((data) => {
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
    this.getLoanTypes();
  }

  toggleExpansion(loanType: LoanType) {
    if (this.expandedLoanType === loanType) {
      this.expandedLoanType = null;
    } else {
      this.expandedLoanType = loanType;
    }
  }

  editLoanType(loanType: LoanType) {
    this.dialog
      .open(EditLoanTypeComponent, {
        data: {
          loanTypeId: loanType.id,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.getLoanTypes());
  }

  deleteLoanType(loanType: LoanType) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Loan Type',
        content: 'Are you sure you want to delete this loan type?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) {
        return;
      }

      this.service.delete(loanType.id).subscribe({
        next: () => {
          Swal.fire({
            title: 'Deleted',
            text: 'The loan type was successfully deleted.',
            icon: 'success',
            showConfirmButton: true,
          });

          this.getLoanTypes();
        },
        error: (err) =>
          this.errorHandler.badRequest(
            err,
            'Failed to delete official business.'
          ),
      });
    });
  }

  newLoanType() {
    this.dialog
      .open(NewLoanTypeComponent)
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.getLoanTypes());
  }
}
