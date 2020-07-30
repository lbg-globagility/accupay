import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Employee } from 'src/app/employees/shared/employee';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Router } from '@angular/router';
import { auditTime, filter } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { EmployeePageOptions } from 'src/app/employees/shared/employee-page-options';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';
import { EmployeeImportParserOutput } from '../shared/employee-import-parser-output';
import { EmployeeImportModel } from '../shared/employee-import-model';
import { PostImportParserOutputDialogComponent } from 'src/app/shared/import/post-import-parser-output-dialog/post-import-parser-output-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss'],
  host: {
    class: 'h-full',
  },
})
export class EmployeesComponent implements OnInit {
  @ViewChild('uploader') fileInput: ElementRef;

  @ViewChild('employeesRef')
  employeesRef: ElementRef;

  searchTerm: string;

  filter: string = 'Active only';

  pageIndex = 0;

  pageSize = 25;

  employees: Employee[];

  totalCount: number;

  modelChanged: Subject<void>;

  selectedEmployees: Employee[];

  selectedEmployee: Employee;
  isDownloadingTemplate: boolean;

  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadEmployees());
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  applyFilter(): void {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox(): void {
    this.searchTerm = null;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  page(pageEvent: PageEvent): void {
    console.log(pageEvent);
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadEmployees();
  }

  selectEmployee(): void {
    const employee =
      this.selectedEmployees.length > 0 ? this.selectedEmployees[0] : null;

    this.router.navigate(['employees', employee.id]);
  }

  private loadEmployees(): void {
    const options = new EmployeePageOptions(
      this.pageIndex,
      this.pageSize,
      this.searchTerm,
      this.filter
    );

    this.employeeService.list(options).subscribe((data) => {
      this.employees = data.items;
      this.totalCount = data.totalCount;
      this.resetScroll();

      this.selectedEmployee = this.employees[0];
      this.selectedEmployees = [this.selectedEmployee];
      this.router.navigate(['employees', this.selectedEmployee.id]);
    });
  }

  private resetScroll(): void {
    this.employeesRef.nativeElement.scrollTo(0, 0);
  }

  downloadTemplate(): void {
    this.isDownloadingTemplate = true;
    this.employeeService
      .getEmployeeTemplate()
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading employee template.'
        );
      })
      .finally(() => {
        this.isDownloadingTemplate = false;
      });
  }

  onImport(files: FileList) {
    const file = files[0];

    this.employeeService.import(file).subscribe(
      (outputParse) => {
        this.displaySuccess(outputParse);
        this.clearFile();
      },
      (err) => {
        this.errorHandler.badRequest(err, 'Failed to import shift.');
        this.clearFile();
      }
    );
  }

  private displaySuccess(outputParse: EmployeeImportParserOutput) {
    const model: EmployeeImportModel = {
      employeeNo: '',
      lastName: '',
      firstName: '',
      middleName: '',
      birthdate: new Date(),
      gender: '',
      nickname: '',
      maritalStatus: '',
      salutation: '',
      address: '',
      mobileNo: '',
      jobPosition: '',
      tin: '',
      sssNo: '',
      philHealthNo: '',
      pagIbigNo: '',
      startDate: new Date(),
      employeeType: '',
      employmentStatus: '',
      leaveAllowance: 0,
      sickLeaveAllowance: 0,
      workDaysPerYear: 0,
      branch: '',
      currentLeaveBalance: 0,
      currentSickLeaveBalance: 0,
      atmNo: '',
      remarks: '',
    };

    this.dialog
      .open(PostImportParserOutputDialogComponent, {
        data: {
          columnHeaders: new EmployeeImportParserOutput().columnHeaders,
          invalidRecords: outputParse.invalidRecords,
          validRecords: outputParse.validRecords,
          propertyNames: Object.getOwnPropertyNames(model),
        },
      })
      .afterClosed()
      .subscribe(() => this.loadEmployees());
  }

  clearFile() {
    this.fileInput.nativeElement.value = '';
  }
}
