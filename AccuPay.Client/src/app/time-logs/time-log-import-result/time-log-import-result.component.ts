import { Component, OnInit, Inject } from '@angular/core';
import { TimeLogImportDetails } from '../shared/time-log-import-details';
import { TimeLogImportResult } from '../shared/time-log-import-result';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TimeLogService } from '../time-log.service';
import { TimeLog } from '../shared/time-log';
import { PageEvent } from '@angular/material/paginator';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-time-log-import-result',
  templateUrl: './time-log-import-result.component.html',
  styleUrls: ['./time-log-import-result.component.scss'],
})
export class TimeLogImportResultComponent implements OnInit {
  readonly displayedColumnsInvalid: string[] = [
    'lineNumber',
    'lineContent',
    'errorMessage',
  ];

  readonly displayedColumnsTimeLog: string[] = [
    'employee',
    'date',
    'timeIn',
    'timeOut',
  ];

  pageIndexSuccess = 0;

  pageSizeSuccess: number = 10;

  pageIndexInvalid = 0;

  pageSizeInvalid: number = 10;

  result: TimeLogImportResult;

  dataSourceSuccess: TimeLog[];

  dataSourceInvalid: TimeLogImportDetails[];

  isSaving: boolean = false;

  constructor(
    private dialog: MatDialogRef<TimeLogImportResult>,
    private timeLogService: TimeLogService,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private errorHandler: ErrorHandler
  ) {
    this.result = data.result;
  }

  ngOnInit(): void {
    console.log(this.result);
    this.pageTimeLogsCompute(this.result.generatedTimeLogs);
    this.pageInvalidCompute(this.result.invalidRecords);
  }

  onPageChangedSuccess(pageEvent: PageEvent, dataSource: any) {
    this.pageIndexSuccess = pageEvent.pageIndex;
    this.pageSizeSuccess = pageEvent.pageSize;

    this.pageTimeLogsCompute(dataSource);
  }

  onPageChangedInvalid(pageEvent: PageEvent, dataSource: any) {
    this.pageIndexInvalid = pageEvent.pageIndex;
    this.pageSizeInvalid = pageEvent.pageSize;

    this.pageInvalidCompute(dataSource);
  }

  pageTimeLogsCompute(dataSource: any): void {
    var pageRange = this.pageIndexSuccess * this.pageSizeSuccess;

    this.dataSourceSuccess = dataSource.slice(
      pageRange,
      pageRange + this.pageSizeSuccess
    );
  }

  pageInvalidCompute(dataSource: any): void {
    var pageRange = this.pageIndexInvalid * this.pageSizeInvalid;

    this.dataSourceInvalid = dataSource.slice(
      pageRange,
      pageRange + this.pageSizeInvalid
    );
  }

  onSave(): void {
    this.isSaving = true;

    this.timeLogService
      .batchApply(this.result.generatedTimeLogs)
      .subscribe(
        (result) => {
          this.dialog.close(true);
        },
        (err) =>
          this.errorHandler.badRequest(
            err,
            'Failed to save imported time logs.'
          )
      )
      .add(() => {
        this.isSaving = false;
      });
  }
}
