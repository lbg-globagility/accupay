import { Component, OnInit, Inject } from '@angular/core';
import { TimeLogImportDetails } from '../shared/time-log-import-details';
import { TimeLogImportResult } from '../shared/time-log-import-result';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TimeLogService } from '../time-log.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-time-log-import-result',
  templateUrl: './time-log-import-result.component.html',
  styleUrls: ['./time-log-import-result.component.scss'],
})
export class TimeLogImportResultComponent implements OnInit {
  readonly displayedColumnsInvalid: string[] = [
    'errorMessage',
    'lineContent',
    'lineNumber',
  ];

  readonly displayedColumnsTimeLog: string[] = [
    'employeeNumber',
    'employeeName',
    'date',
    'time',
  ];

  displayedColumns: string[] = this.displayedColumnsTimeLog;

  type: string;

  result: TimeLogImportResult;

  dataSource: any;

  constructor(
    private dialog: MatDialogRef<TimeLogImportResult>,
    private timeLogService: TimeLogService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.result = data.result;
  }

  ngOnInit(): void {
    console.log(this.result);
    this.dataSource = this.result.generatedTimeLogs;
  }

  successButton(): void {
    this.dataSource = this.result.generatedTimeLogs;
    this.displayedColumns = this.displayedColumnsTimeLog;
  }

  errorButton(): void {
    this.dataSource = this.result.invalidRecords;
    this.displayedColumns = this.displayedColumnsInvalid;
  }

  onSave(): void {
    this.dialog.close(true);
  }
}
