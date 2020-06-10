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
    'workHours',
    'overtimeHours',
    'nightDiffHours',
    'nightDiffOTHours',
    'lateHours',
    'undertimeHours',
    'absentHours',
  ];

  dataSource: MatTableDataSource<TimeEntry>;

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    this.dataSource = new MatTableDataSource(this.timeEntries);
  }
}
