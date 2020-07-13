import { Component, OnInit, Inject } from '@angular/core';
import { Shift } from 'src/app/shifts/shared/shift';
import { ShiftService } from 'src/app/shifts/shift.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import * as moment from 'moment';
import { LoadingState } from 'src/app/core/states/loading-state';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { range } from 'src/app/core/functions/dates';
import { TimeParser } from 'src/app/core/shared/services/time-parser';

@Component({
  selector: 'app-edit-shift',
  templateUrl: './edit-shift.component.html',
  styleUrls: ['./edit-shift.component.scss'],
})
export class EditShiftComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'date',
    'startTime',
    'endTime',
    'breakStartTime',
    'breakLength',
  ];

  savingState: LoadingState = new LoadingState();

  employee: Shift;

  dateFrom: Date;

  dateTo: Date;

  form: FormGroup = this.fb.group({
    shifts: this.fb.array([]),
  });

  get shiftsArray(): FormArray {
    return this.form.get('shifts') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private shiftService: ShiftService,
    private dialogRef: MatDialogRef<EditShiftComponent>,
    private errorHandler: ErrorHandler,
    private timeParser: TimeParser,
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

    const shifts = [];
    for (const data of value.shifts) {
      const dateMoment = moment(data.date);
      const shift = {
        employeeId: this.employee.employeeId,
        date: data.date,
        startTime: this.timeParser.parse(dateMoment, data.startTime),
        endTime: this.timeParser.parse(dateMoment, data.endTime),
        breakStartTime: this.timeParser.parse(dateMoment, data.breakStartTime),
        breakLength: data.breakLength == null ? 0 : data.breakLength,
        isOffset: data.isOffset == null ? false : Boolean(data.isOffset),
      };

      console.log(shift.startTime, shift.endTime);

      shifts.push(shift);
    }

    // console.log(shifts);
    // console.log(value.shifts);
    this.shiftService.batchApply(shifts).subscribe({
      next: () => {
        this.savingState.changeToSuccess();
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to save shifts');
      },
    });
  }

  private initializeForm(): void {
    for (const date of range(this.dateFrom, this.dateTo)) {
      const group = this.fb.group({
        date: [date],
        startTime: [],
        endTime: [],
        breakStartTime: [],
        breakLength: [],
        isOffset: [],
      });

      // Find shift for the current row, if found, patch the time in and out.
      const shift = this.employee.shifts.find((t) => {
        return t.date.substring(0, 10) === moment(date).format('yyyy-MM-DD');
      });

      if (shift) {
        group.patchValue({
          startTime: this.momentTimeFormatted(shift.startTime),
          endTime: this.momentTimeFormatted(shift.endTime),
          breakStartTime: this.momentTimeFormatted(shift.breakStartTime),
          breakLength: shift.breakLength,
          isOffset: shift.isOffset,
        });
      }

      this.shiftsArray.push(group);
    }
  }

  private momentTimeFormatted(input: Date): any {
    return moment(input).format('HH:mm');
  }
}
