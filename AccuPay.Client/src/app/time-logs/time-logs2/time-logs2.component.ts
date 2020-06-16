import { Component, OnInit } from '@angular/core';
import { TimeLogService } from 'src/app/time-logs/time-log.service';
import { Moment } from 'moment';
import * as moment from 'moment';
import { PageOptions } from 'src/app/core/shared/page-options';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeTimeLogs } from 'src/app/time-logs/shared/employee-time-logs';

interface TimeLogDate {
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
  readonly defaultColumns = ['employee'];

  displayedColumns = ['employee'];

  dateFrom: Moment = moment(new Date(2020, 1, 1));

  dateTo: Moment = moment(new Date(2020, 1, 15));

  dates: TimeLogDate[] = [];

  dataSource: any[] = [{}, {}, {}, {}, {}, {}];

  dataSource2: MatTableDataSource<EmployeeTimeLogs> = new MatTableDataSource();

  constructor(private timeLogService: TimeLogService) {}

  ngOnInit(): void {
    const options = new PageOptions(1, 25, null, null);

    this.timeLogService
      .listByEmployee(options, this.dateFrom.toDate(), this.dateTo.toDate())
      .subscribe((data) => {
        this.dataSource2 = new MatTableDataSource(data.items);
      });

    const dates: TimeLogDate[] = [];

    let current = this.dateFrom.clone();
    const last = this.dateTo.add(1, 'days');
    while (current.isBefore(last)) {
      dates.push({
        title: current.format('MM/DD'),
        date: current.toDate(),
        dayOfWeek: current.format('ddd'),
      });

      current = current.add(1, 'days');
    }

    this.dates = dates;
    this.displayedColumns = [
      ...this.defaultColumns,
      ...this.dates.map((t) => t.title),
    ];
  }

  getTimeIn(employee: EmployeeTimeLogs, date: Date) {
    return employee.timeLogs.find((t) => t.date === date.toISOString())
      ?.startTime;
  }

  getTimeOut(employee: EmployeeTimeLogs, date: Date) {
    return employee.timeLogs.find((t) => t.date === date.toISOString())
      ?.endTime;
  }
}
