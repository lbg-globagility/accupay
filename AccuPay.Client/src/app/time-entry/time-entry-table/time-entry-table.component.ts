import {
  Component,
  OnInit,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TimeEntry } from 'src/app/time-entry/shared/time-entry';

@Component({
  selector: 'app-time-entry-table',
  templateUrl: './time-entry-table.component.html',
  styleUrls: ['./time-entry-table.component.scss'],
})
export class TimeEntryTableComponent implements OnChanges {
  @Input()
  timeEntries: TimeEntry[] = [];

  readonly displayedColumns = [
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
    'lateHours',
    'lateDeduction',
    'undertimeHours',
    'undertimeDeduction',
    'absentHours',
    'absentDeduction',
  ];

  dataSource: MatTableDataSource<TimeEntry>;

  ngOnChanges(changes: SimpleChanges): void {
    this.dataSource = new MatTableDataSource(this.timeEntries);
  }
}
