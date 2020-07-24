import { auditTime, filter } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';
import { NewOfficialBusinessComponent } from '../new-official-business/new-official-business.component';
import { MatDialog } from '@angular/material/dialog';
import { EditOfficialBusinessComponent } from '../edit-official-business/edit-official-business.component';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Moment } from 'moment';
import { PermissionTypes } from 'src/app/core/auth';

@Component({
  selector: 'app-official-business-list',
  templateUrl: './official-business-list.component.html',
  styleUrls: ['./official-business-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class OfficialBusinessListComponent implements OnInit {
  readonly PermissionTypes = PermissionTypes;

  readonly displayedColumns: string[] = [
    'employee',
    'date',
    'time',
    'status',
    'actions',
  ];

  expandedOfficialBusiness: OfficialBusiness;

  searchTerm: string;

  dateFrom: Moment;

  dateTo: Moment;

  modelChanged: Subject<any>;

  totalCount: number;

  dataSource: MatTableDataSource<OfficialBusiness>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  isDownloadingTemplate: boolean;

  constructor(
    private officialBusinessService: OfficialBusinessService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getOfficialBusinessList());
  }

  ngOnInit(): void {
    this.getOfficialBusinessList();
  }

  getOfficialBusinessList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.officialBusinessService
      .getAll(options, this.searchTerm, this.dateFrom, this.dateTo)
      .subscribe((data) => {
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
    this.getOfficialBusinessList();
  }

  toggleExpansion(officialBusiness: OfficialBusiness) {
    if (this.expandedOfficialBusiness === officialBusiness) {
      this.expandedOfficialBusiness = null;
    } else {
      this.expandedOfficialBusiness = officialBusiness;
    }
  }

  editOfficialBusiness(officialBusiness: OfficialBusiness) {
    this.dialog
      .open(EditOfficialBusinessComponent, {
        data: {
          officialBusinessId: officialBusiness.id,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.getOfficialBusinessList());
  }

  deleteOfficialBusiness(officialBusiness: OfficialBusiness) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Official Business',
        content: 'Are you sure you want to delete this official business?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) {
        return;
      }

      this.officialBusinessService.delete(officialBusiness.id).subscribe({
        next: () => {
          Swal.fire({
            title: 'Deleted',
            text: 'The official business was successfully deleted.',
            icon: 'success',
            showConfirmButton: true,
          });

          this.getOfficialBusinessList();
        },
        error: (err) =>
          this.errorHandler.badRequest(
            err,
            'Failed to delete official business.'
          ),
      });
    });
  }
  newOfficialBusiness() {
    this.dialog
      .open(NewOfficialBusinessComponent)
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.getOfficialBusinessList());
  }

  downloadTemplate(): void {
    this.isDownloadingTemplate = true;
    this.officialBusinessService
      .getOfficialBusTemplate()
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading official business template.'
        );
      })
      .finally(() => {
        this.isDownloadingTemplate = false;
      });
  }
}
