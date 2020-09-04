import { Component, OnInit } from '@angular/core';
import { TimeLogService } from 'src/app/time-logs/time-log.service';
import * as moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeTimeLogs } from 'src/app/time-logs/shared/employee-time-logs';
import { PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { auditTime, filter } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { MatDialog } from '@angular/material/dialog';
import { EditTimeLogComponent } from 'src/app/time-logs/edit-time-log/edit-time-log.component';
import { range } from 'src/app/core/functions/dates';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { TimeLogImportResultComponent } from '../time-log-import-result/time-log-import-result.component';
import Swal from 'sweetalert2';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { TimeLogsByEmployeePageOptions } from 'src/app/time-logs/shared/timelogs-by-employee-page-options';
import { PermissionTypes } from 'src/app/core/auth';

interface DateHeader {
  title: string;
  date: Date;
  dateOnly: string;
  dayOfWeek: string;
}

interface EmployeeTimeLogsModel {
  employeeId: number;
  employeeNo: string;
  fullName: string;
  timeLogs: {};
}

@Component({
  selector: 'app-time-logs2',
  templateUrl: './time-logs.component.html',
  styleUrls: ['./time-logs.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class TimeLogsComponent implements OnInit {
  readonly PermissionTypes = PermissionTypes;

  displayedColumns = ['employee'];

  dateFrom: Date = new Date();

  dateTo: Date = new Date();

  headers: DateHeader[] = [];

  pageIndex = 0;

  pageSize: number = 10;

  modelChanged: Subject<any>;

  searchTerm: string = '';

  statusFilter: string = 'Active only';

  dataSource: MatTableDataSource<
    EmployeeTimeLogsModel
  > = new MatTableDataSource();

  totalCount: number;

  employees: EmployeeTimeLogs[] = [];

  constructor(
    private timeLogService: TimeLogService,
    private payPeriodService: PayPeriodService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler,
    private snackBar: MatSnackBar
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadTimeLogs());
  }

  ngOnInit(): void {
    this.payPeriodService.getLatest().subscribe((payPeriod) => {
      this.dateFrom = new Date(payPeriod.cutoffStart);
      this.dateTo = new Date(payPeriod.cutoffEnd);
      this.createDateHeaders();
      this.loadTimeLogs();
    });
  }

  datesChanged() {
    this.createDateHeaders();
    this.loadTimeLogs();
  }

  applyFilter(): void {
    this.modelChanged.next();
  }

  clearSearchBox(): void {
    this.searchTerm = '';
    this.modelChanged.next();
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.modelChanged.next();
  }

  edit(model: EmployeeTimeLogsModel): void {
    const employee = this.employees.find(
      (t) => t.employeeId === model.employeeId
    );

    this.dialog
      .open(EditTimeLogComponent, {
        data: {
          employee,
          dateFrom: this.dateFrom,
          dateTo: this.dateTo,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => {
        this.loadTimeLogs();
        this.snackBar.open('Successful update', null, {
          duration: 2000,
        });
      });
  }

  onImport(files: FileList) {
    const file = files[0];
    this.timeLogService.import(file).subscribe(
      (data) => {
        this.dialog
          .open(TimeLogImportResultComponent, {
            data: {
              result: data,
            },
          })
          .afterClosed()
          .pipe(filter((t) => t))
          .subscribe(() => {
            this.loadTimeLogs();
            this.displaySuccess();
          });
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to import time logs.')
    );
  }

  private loadTimeLogs(): void {
    const options = new TimeLogsByEmployeePageOptions(
      this.pageIndex,
      this.pageSize,
      this.dateFrom,
      this.dateTo,
      this.searchTerm,
      this.statusFilter
    );

    this.timeLogService.listByEmployee(options).subscribe((data) => {
      this.employees = data.items;
      this.dataSource = new MatTableDataSource(
        this.convertToLocalModels(data.items)
      );
      this.totalCount = data.totalCount;
    });
  }

  private convertToLocalModels(employees: EmployeeTimeLogs[]) {
    return employees.map((t) => this.convertToLocalModel(t));
  }

  private convertToLocalModel(employee: EmployeeTimeLogs) {
    const model: EmployeeTimeLogsModel = {
      employeeId: employee.employeeId,
      employeeNo: employee.employeeNo,
      fullName: employee.fullName,
      timeLogs: {},
    };

    for (const timeLog of employee.timeLogs) {
      model.timeLogs[timeLog.date.substring(0, 10)] = {
        id: timeLog.id,
        date: timeLog.date,
        startTime: timeLog.startTime,
        endTime: timeLog.endTime,
      };
    }

    return model;
  }

  private createDateHeaders(): void {
    this.headers = range(this.dateFrom, this.dateTo).map((date) => ({
      title: moment(date).format('MM/DD'),
      date,
      dateOnly: moment(date).format('yyyy-MM-DD'),
      dayOfWeek: moment(date).format('ddd'),
    }));

    this.displayedColumns = [
      'employee',
      ...this.headers.map((t) => t.title),
      'actions',
    ];
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
