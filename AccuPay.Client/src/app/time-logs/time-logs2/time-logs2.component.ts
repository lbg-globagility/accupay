import { Component, OnInit } from '@angular/core';
import { TimeLogService } from 'src/app/time-logs/time-log.service';
import { Moment } from 'moment';
import * as moment from 'moment';
import { PageOptions } from 'src/app/core/shared/page-options';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeTimeLogs } from 'src/app/time-logs/shared/employee-time-logs';
import { PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { MatDialog } from '@angular/material/dialog';
import { EditTimeLogComponent } from 'src/app/time-logs/edit-time-log/edit-time-log.component';
import { range } from 'src/app/core/functions/dates';

interface DateHeader {
  title: string;
  date: Date;
  dayOfWeek: string;
}

@Component({
  selector: 'app-time-logs2',
  templateUrl: './time-logs2.component.html',
  styleUrls: ['./time-logs2.component.scss'],
})
export class TimeLogs2Component implements OnInit {
  displayedColumns = ['employee'];

  dateFrom = new Date(2020, 1, 1);

  dateTo = new Date(2020, 1, 15);

  headers: DateHeader[] = [];

  pageIndex = 0;

  pageSize: number = 10;

  modelChanged: Subject<any>;

  dataSource: MatTableDataSource<EmployeeTimeLogs> = new MatTableDataSource();

  totalCount: number;

  constructor(
    private timeLogService: TimeLogService,
    private dialog: MatDialog
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadTimeLogs());
  }

  ngOnInit(): void {
    this.createDateHeaders();
    this.loadTimeLogs();
  }

  getTimeIn(employee: EmployeeTimeLogs, date: Date): string {
    return employee.timeLogs.find((t) => t.date === date.toISOString())
      ?.startTime;
  }

  getTimeOut(employee: EmployeeTimeLogs, date: Date): string {
    return employee.timeLogs.find((t) => t.date === date.toISOString())
      ?.endTime;
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.modelChanged.next();
  }

  edit(employee: EmployeeTimeLogs): void {
    this.dialog.open(EditTimeLogComponent, {
      data: {
        employee,
        dateFrom: this.dateFrom,
        dateTo: this.dateTo,
      },
    });
  }

  private loadTimeLogs(): void {
    const options = new PageOptions(this.pageIndex, this.pageSize);

    this.timeLogService
      .listByEmployee(options, this.dateFrom, this.dateTo)
      .subscribe((data) => {
        this.dataSource = new MatTableDataSource(data.items);
        this.totalCount = data.totalCount;
      });
  }

  private createDateHeaders(): void {
    this.headers = range(this.dateFrom, this.dateTo).map((date) => ({
      title: moment(date).format('MM/DD'),
      date,
      dayOfWeek: moment(date).format('ddd'),
    }));

    this.displayedColumns = [
      'employee',
      ...this.headers.map((t) => t.title),
      'actions',
    ];
  }
}
