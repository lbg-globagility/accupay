import { Component, OnInit, Inject } from '@angular/core';
import { TimeLogService } from '../time-log.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EmployeeTimeLogs } from 'src/app/time-logs/shared/employee-time-logs';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import * as moment from 'moment';
import { range } from 'src/app/core/functions/dates';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import { LoadingState } from 'src/app/core/states/loading-state';

@Component({
  selector: 'app-edit-time-log',
  templateUrl: './edit-time-log.component.html',
  styleUrls: ['./edit-time-log.component.scss'],
})
export class EditTimeLogComponent implements OnInit {
  readonly displayedColumns: string[] = ['date', 'timeIn', 'timeOut'];

  savingState: LoadingState = new LoadingState();

  employee: EmployeeTimeLogs;

  dateFrom: Date;

  dateTo: Date;

  form: FormGroup = this.fb.group({
    timeLogs: this.fb.array([]),
  });

  get timeLogsArray(): FormArray {
    return this.form.get('timeLogs') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private timeParser: TimeParser,
    private timeLogService: TimeLogService,
    private dialogRef: MatDialogRef<EditTimeLogComponent>,
    private errorHandler: ErrorHandler,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.employee = data.employee;
    this.dateFrom = data.dateFrom;
    this.dateTo = data.dateTo;
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  save(): void {
    this.savingState.changeToLoading();

    const value = this.form.value;

    const timeLogs = [];
    for (const data of value.timeLogs) {
      const timeLog = {
        employeeId: this.employee.employeeId,
        date: data.date,
        startTime: this.timeParser.parse(moment(data.date), data.timeIn),
        endTime: this.timeParser.parse(moment(data.date), data.timeOut),
      };

      timeLogs.push(timeLog);
    }

    this.timeLogService.update2(timeLogs).subscribe({
      next: () => {
        this.savingState.changeToSuccess();
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to save time logs');
      },
    });
  }

  private initializeForm(): void {
    for (const date of range(this.dateFrom, this.dateTo)) {
      const group = this.fb.group({
        date: [date],
        timeIn: [],
        timeOut: [],
      });

      // Find time log for the current row, if found, patch the time in and out.
      const timeLog = this.employee.timeLogs.find(
        (t) => t.date === date.toISOString()
      );
      if (timeLog) {
        group.patchValue({
          timeIn: moment(timeLog.startTime).format('HH:mm'),
          timeOut: moment(timeLog.endTime).format('HH:mm'),
        });
      }

      this.timeLogsArray.push(group);
    }
  }
}
