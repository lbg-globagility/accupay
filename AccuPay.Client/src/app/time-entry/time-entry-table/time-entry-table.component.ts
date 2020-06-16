import { Component, Input, OnChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TimeEntry } from 'src/app/time-entry/shared/time-entry';
import { filter, sumBy } from 'lodash';

enum FilterType {
  Number,
  Object,
  Array,
}

@Component({
  selector: 'app-time-entry-table',
  templateUrl: './time-entry-table.component.html',
  styleUrls: ['./time-entry-table.component.scss'],
})
export class TimeEntryTableComponent implements OnChanges {
  @Input()
  timeEntries: TimeEntry[] = [];

  readonly allColumns = [
    'date',
    'shift',
    'timeLog',
    'officialBusiness',
    'regularHours',
    'leave',
    'leaveHours',
    'leavePay',
    'overtimes',
    'overtimeHours',
    'overtimePay',
    'nightDiffHours',
    'nightDiffPay',
    'nightDiffOTHours',
    'nightDiffOTPay',
    'restDayHours',
    'restDayAmount',
    'restDayOTHours',
    'restDayOTPay',
    'specialHolidayHours',
    'specialHolidayPay',
    'specialHolidayOTHours',
    'specialHolidayOTPay',
    'regularHolidayHours',
    'regularHolidayPay',
    'regularHolidayOTHours',
    'regularHolidayOTPay',
    'lateHours',
    'lateDeduction',
    'undertimeHours',
    'undertimeDeduction',
    'absentHours',
    'absentDeduction',
  ];

  totalColumn = {
    regularHours: 0,
    leaveHours: 0,
    leavePay: 0,
    overtimeHours: 0,
    overtimePay: 0,
    nightDiffHours: 0,
    nightDiffPay: 0,
    nightDiffOTHours: 0,
    nightDiffOTPay: 0,
    restDayHours: 0,
    restDayAmount: 0,
    restDayOTHours: 0,
    restDayOTPay: 0,
    specialHolidayHours: 0,
    specialHolidayPay: 0,
    specialHolidayOTHours: 0,
    specialHolidayOTPay: 0,
    regularHolidayHours: 0,
    regularHolidayPay: 0,
    regularHolidayOTHours: 0,
    regularHolidayOTPay: 0,
    lateHours: 0,
    lateDeduction: 0,
    undertimeHours: 0,
    undertimeDeduction: 0,
    absentHours: 0,
    absentDeduction: 0,
  };

  displayedColumns: string[];

  dataSource: MatTableDataSource<TimeEntry>;

  constructor() {
    this.displayedColumns = this.allColumns;
  }
  ngOnChanges(): void {
    if (this.timeEntries == null) return;

    this.displayedColumns = this.allColumns.slice();
    this.hideEmptyColumns();
    this.sumColumnValues();
    this.dataSource = new MatTableDataSource(this.timeEntries);
  }

  private hideEmptyColumns(): void {
    this.hideColumn('officialBusiness', FilterType.Object);
    this.hideColumn('leave', FilterType.Object);
    this.hideColumn('leaveHours', FilterType.Number);
    this.hideColumn('leavePay', FilterType.Number);
    this.hideColumn('overtimes', FilterType.Array);
    this.hideColumn('overtimeHours', FilterType.Number);
    this.hideColumn('overtimePay', FilterType.Number);
    this.hideColumn('nightDiffHours', FilterType.Number);
    this.hideColumn('nightDiffPay', FilterType.Number);
    this.hideColumn('nightDiffOTHours', FilterType.Number);
    this.hideColumn('nightDiffOTPay', FilterType.Number);
    this.hideColumn('restDayHours', FilterType.Number);
    this.hideColumn('restDayAmount', FilterType.Number);
    this.hideColumn('restDayOTHours', FilterType.Number);
    this.hideColumn('restDayOTPay', FilterType.Number);
    this.hideColumn('specialHolidayHours', FilterType.Number);
    this.hideColumn('specialHolidayPay', FilterType.Number);
    this.hideColumn('specialHolidayOTHours', FilterType.Number);
    this.hideColumn('specialHolidayOTPay', FilterType.Number);
    this.hideColumn('regularHolidayHours', FilterType.Number);
    this.hideColumn('regularHolidayPay', FilterType.Number);
    this.hideColumn('regularHolidayOTHours', FilterType.Number);
    this.hideColumn('regularHolidayOTPay', FilterType.Number);
  }

  private sumColumnValues(): void {
    this.sumColumn('regularHours');
    this.sumColumn('leaveHours');
    this.sumColumn('leavePay');
    this.sumColumn('overtimeHours');
    this.sumColumn('overtimePay');
    this.sumColumn('nightDiffHours');
    this.sumColumn('nightDiffPay');
    this.sumColumn('nightDiffOTHours');
    this.sumColumn('nightDiffOTPay');
    this.sumColumn('restDayHours');
    this.sumColumn('restDayAmount');
    this.sumColumn('restDayOTHours');
    this.sumColumn('restDayOTPay');
    this.sumColumn('specialHolidayHours');
    this.sumColumn('specialHolidayPay');
    this.sumColumn('specialHolidayOTHours');
    this.sumColumn('specialHolidayOTPay');
    this.sumColumn('regularHolidayHours');
    this.sumColumn('regularHolidayPay');
    this.sumColumn('regularHolidayOTHours');
    this.sumColumn('regularHolidayOTPay');
    this.sumColumn('lateHours');
    this.sumColumn('lateDeduction');
    this.sumColumn('undertimeHours');
    this.sumColumn('undertimeDeduction');
    this.sumColumn('absentHours');
    this.sumColumn('absentDeduction');
  }

  private hideColumn(key: string, type: FilterType): void {
    let condition: Function;

    switch (type) {
      case FilterType.Number:
        condition = (t) => t[key] != null && t[key] != 0;
        break;

      case FilterType.Array:
        condition = (t) => t[key] != null && t[key].length > 0;
        break;

      case FilterType.Object:
        condition = (t) => t[key] != null;
        break;
    }

    if (filter(this.timeEntries, condition).length == 0) {
      this.displayedColumns.splice(this.displayedColumns.indexOf(key), 1);
    }
  }

  private sumColumn(key: string): void {
    let sum = sumBy(this.timeEntries, key);
    console.log(key);
    console.log(sum);
    this.totalColumn[key] = sum;
    console.log(this.totalColumn);
  }
}
