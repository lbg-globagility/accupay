import { Component, Input, OnChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TimeEntry } from 'src/app/time-entry/shared/time-entry';
import { filter } from 'lodash';

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

  displayedColumns: string[];

  dataSource: MatTableDataSource<TimeEntry>;

  constructor() {
    this.displayedColumns = this.allColumns;
  }
  ngOnChanges(): void {
    if (this.timeEntries == null) return;

    this.displayedColumns = this.allColumns.slice();
    this.unHideColumn('officialBusiness', FilterType.Object);
    this.unHideColumn('leave', FilterType.Object);
    this.unHideColumn('leaveHours', FilterType.Number);
    this.unHideColumn('leavePay', FilterType.Number);
    this.unHideColumn('overtimes', FilterType.Array);
    this.unHideColumn('overtimeHours', FilterType.Number);
    this.unHideColumn('overtimePay', FilterType.Number);
    this.unHideColumn('nightDiffHours', FilterType.Number);
    this.unHideColumn('nightDiffPay', FilterType.Number);
    this.unHideColumn('nightDiffOTHours', FilterType.Number);
    this.unHideColumn('nightDiffOTPay', FilterType.Number);
    this.unHideColumn('restDayHours', FilterType.Number);
    this.unHideColumn('restDayAmount', FilterType.Number);
    this.unHideColumn('restDayOTHours', FilterType.Number);
    this.unHideColumn('restDayOTPay', FilterType.Number);
    this.unHideColumn('specialHolidayHours', FilterType.Number);
    this.unHideColumn('specialHolidayPay', FilterType.Number);
    this.unHideColumn('specialHolidayOTHours', FilterType.Number);
    this.unHideColumn('specialHolidayOTPay', FilterType.Number);
    this.unHideColumn('regularHolidayHours', FilterType.Number);
    this.unHideColumn('regularHolidayPay', FilterType.Number);
    this.unHideColumn('regularHolidayOTHours', FilterType.Number);
    this.unHideColumn('regularHolidayOTPay', FilterType.Number);
    this.dataSource = new MatTableDataSource(this.timeEntries);
  }

  private unHideColumn(key: string, type: FilterType) {
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

    // var fil = filter(this.timeEntries, (t) => {
    //   return t.leave != null;
    // });

    if (filter(this.timeEntries, condition).length == 0) {
      this.displayedColumns.splice(this.displayedColumns.indexOf(key), 1);
    }
  }
}
