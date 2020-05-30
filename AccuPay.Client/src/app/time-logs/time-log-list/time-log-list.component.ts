import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { TimeLog } from 'src/app/time-logs/shared/time-log';
import { TimeLogService } from 'src/app/time-logs/time-log.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-time-log-list',
  templateUrl: './time-log-list.component.html',
  styleUrls: ['./time-log-list.component.scss'],
})
export class TimeLogListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employeeNumber',
    'employeeName',
    'date',
    'time',
    'branch',
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  timeLogs: TimeLog[];

  totalCount: number;

  dataSource: MatTableDataSource<TimeLog>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'employeeName',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(
    private timeLogService: TimeLogService,
    private errorHandler: ErrorHandler
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getTimeLogList());
  }

  ngOnInit(): void {
    this.getTimeLogList();
  }

  getTimeLogList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.timeLogService.getAll(options, this.searchTerm).subscribe((data) => {
      this.timeLogs = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.timeLogs);
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
    this.getTimeLogList();
  }

  onImport(files: FileList) {
    const file = files[0];
    this.timeLogService.import(file).subscribe(
      () => {
        this.getTimeLogList();
        this.displaySuccess();
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to import time logs.')
    );
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully imported new time logs!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
