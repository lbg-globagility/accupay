import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { ShiftService } from '../shift.service';
import { auditTime, filter } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageEvent } from '@angular/material/paginator';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { MatDialog } from '@angular/material/dialog';
import * as moment from 'moment';
import { range } from 'src/app/core/functions/dates';
import { EmployeeShifts } from '../shared/employee-shifts';
import { ShiftsByEmployeePageOptions } from '../shared/shifts-by-employee-page-option';
import { EditShiftComponent } from '../edit-shift/edit-shift.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PermissionTypes } from 'src/app/core/auth';
import { PostImportParserOutputDialogComponent } from 'src/app/shared/import/post-import-parser-output-dialog/post-import-parser-output-dialog.component';
import { ShiftImportParserOutput } from '../shared/shift-import-parser-output';
import { ShiftImportModel } from '../shared/shift-import-model';

interface DateHeader {
  title: string;
  date: Date;
  dateOnly: string;
  dayOfWeek: string;
}

interface EmployeeDutySchedulesModel {
  employeeId: number;
  employeeNo: string;
  fullName: string;
  shifts: {};
}

@Component({
  selector: 'app-shift-list',
  templateUrl: './shift-list.component.html',
  styleUrls: ['./shift-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ShiftListComponent implements OnInit {
  @ViewChild('uploader') fileInput: ElementRef;

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
    EmployeeDutySchedulesModel
  > = new MatTableDataSource();

  totalCount: number;

  employees: EmployeeShifts[] = [];
  isDownloadingTemplate: boolean;

  constructor(
    private shiftService: ShiftService,
    private payPeriodService: PayPeriodService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler,
    private snackBar: MatSnackBar
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getShiftList());
  }

  ngOnInit(): void {
    this.payPeriodService.getLatest().subscribe((payPeriod) => {
      this.dateFrom = new Date(payPeriod.cutoffStart);
      this.dateTo = new Date(payPeriod.cutoffEnd);
      this.createDateHeaders();
      this.getShiftList();
    });
  }

  datesChanged() {
    this.createDateHeaders();
    this.getShiftList();
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

  edit(employeeShift: EmployeeShifts): void {
    const employee = this.employees.find(
      (t) => t.employeeId === employeeShift.employeeId
    );

    this.dialog
      .open(EditShiftComponent, {
        data: {
          employee,
          dateFrom: this.dateFrom,
          dateTo: this.dateTo,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => {
        this.getShiftList();
        this.snackBar.open('Successful update', null, {
          duration: 2000,
        });
      });
  }

  onImport(files: FileList) {
    const file = files[0];

    this.shiftService.import(file).subscribe(
      (outputParse) => {
        const model: ShiftImportModel = {
          employeeNo: '',
          fullName: '',
          date: new Date(),
          timeFromDisplay: new Date(),
          timeToDisplay: new Date(),
          breakFromDisplay: new Date(),
          breakLength: 0,
          isRestDayText: false,
          remarks: '',
        };

        this.dialog
          .open(PostImportParserOutputDialogComponent, {
            data: {
              columnHeaders: new ShiftImportParserOutput().columnHeaders,
              invalidRecords: outputParse.invalidRecords,
              validRecords: outputParse.validRecords,
              propertyNames: Object.getOwnPropertyNames(model),
            },
          })
          .afterClosed()
          .subscribe(() => {
            this.getShiftList();
            this.displaySuccess();
            this.clearFile();
          });
      },
      (err) => {
        this.errorHandler.badRequest(err, 'Failed to import shift.');
        this.clearFile();
      }
    );
  }

  private getShiftList(): void {
    const options = new ShiftsByEmployeePageOptions(
      this.pageIndex,
      this.pageSize,
      this.dateFrom,
      this.dateTo,
      this.searchTerm,
      this.statusFilter
    );

    this.shiftService.listByEmployee(options).subscribe((data) => {
      this.employees = data.items;
      this.dataSource = new MatTableDataSource(
        this.convertToLocalModels(data.items)
      );
      this.totalCount = data.totalCount;
    });
  }

  private convertToLocalModels(employeeShifts: EmployeeShifts[]) {
    return employeeShifts.map((t) => this.convertToLocalModel(t));
  }

  private convertToLocalModel(employeeShift: EmployeeShifts) {
    const model: EmployeeDutySchedulesModel = {
      employeeId: employeeShift.employeeId,
      employeeNo: employeeShift.employeeNo,
      fullName: employeeShift.fullName,
      shifts: {},
    };

    for (const shift of employeeShift.shifts) {
      const dateDate = shift.date.substring(0, 10);
      model.shifts[dateDate] = {
        id: shift.id,
        date: shift.date,
        startTime: shift.startTime,
        endTime: shift.endTime,
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
      text: 'Successfully imported new shifts!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }

  downloadTemplate(): void {
    this.isDownloadingTemplate = true;
    this.shiftService
      .getShifScheduleTemplate()
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading shift schedule template.'
        );
      })
      .finally(() => {
        this.isDownloadingTemplate = false;
      });
  }

  clearFile() {
    this.fileInput.nativeElement.value = '';
  }
}
