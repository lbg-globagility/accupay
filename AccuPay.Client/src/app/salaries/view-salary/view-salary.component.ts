import Swal from 'sweetalert2';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { PageOptions } from 'src/app/core/shared/page-options';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Employee } from 'src/app/employees/shared/employee';
import { SalaryImportParserOutput } from '../shared/salary-import-parser-output';
import { SalaryImportModel } from '../shared/salary-import-model';
import { PostImportParserOutputDialogComponent } from 'src/app/shared/import/post-import-parser-output-dialog/post-import-parser-output-dialog.component';
import { PermissionTypes } from 'src/app/core/auth';

@Component({
  selector: 'app-view-salary',
  templateUrl: './view-salary.component.html',
  styleUrls: ['./view-salary.component.scss'],
})
export class ViewSalaryComponent implements OnInit {
  readonly PermissionTypes = PermissionTypes;

  @ViewChild('uploader') fileInput: ElementRef;

  employeeId: number = Number(this.route.snapshot.paramMap.get('employeeId'));

  latestSalary: Salary = null;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  salaryId = Number(this.route.snapshot.paramMap.get('id'));

  employee: Employee;

  salaries: Salary[] = [];

  displayedColumns = [
    'effectiveFrom',
    'basicAmount',
    'allowanceAmount',
    'totalAmount',
  ];
  isDownloadingTemplate: boolean;

  constructor(
    private salaryService: SalaryService,
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.employeeId = Number(paramMap.get('employeeId'));
      this.loadEmployee();
      this.loadSalary();
      this.loadSalaries();
    });
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Salary',
        content: 'Are you sure you want to delete this salary?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.salaryService.delete(this.salaryId).subscribe(
        () => {
          this.router.navigate(['salaries']);
          Swal.fire({
            title: 'Deleted',
            text: `The salary was successfully deleted.`,
            icon: 'success',
            showConfirmButton: true,
          });
        },
        (err) => this.errorHandler.badRequest(err, 'Failed to delete salary.')
      );
    });
  }

  private loadEmployee(): void {
    this.employeeService
      .getById(this.employeeId)
      .subscribe((employee) => (this.employee = employee));
  }

  private loadSalaries(): void {
    const options = new PageOptions(0, 25);
    this.salaryService
      .list(options, null, this.employeeId)
      .subscribe((data) => {
        this.salaries = data.items;
      });
  }

  private loadSalary(): void {
    this.salaryService.getLatest(this.employeeId).subscribe(
      (salary) => {
        this.latestSalary = salary;

        this.isLoading.next(true);
      },
      () => {
        this.latestSalary = null;

        this.isLoading.next(true);
      }
    );
  }

  downloadTemplate(): void {
    this.isDownloadingTemplate = true;
    this.salaryService
      .getSalaryTemplate()
      .catch((err) => {
        this.errorHandler.badRequest(err, 'Error downloading salary template.');
      })
      .finally(() => {
        this.isDownloadingTemplate = false;
      });
  }

  onImport(files: FileList) {
    const file = files[0];

    this.salaryService.import(file).subscribe(
      (outputParse) => {
        this.displaySuccess(outputParse);
        this.clearFile();
      },
      (err) => {
        this.errorHandler.badRequest(err, 'Failed to import salary.');
        this.clearFile();
      }
    );
  }

  private displaySuccess(outputParse: SalaryImportParserOutput) {
    const model: SalaryImportModel = {
      employeeNo: '',
      employeeName: '',
      effectiveFrom: new Date(),
      basicSalary: 0,
      allowanceSalary: 0,
      remarks: '',
    };

    this.dialog
      .open(PostImportParserOutputDialogComponent, {
        data: {
          columnHeaders: new SalaryImportParserOutput().columnHeaders,
          invalidRecords: outputParse.invalidRecords,
          validRecords: outputParse.validRecords,
          propertyNames: Object.getOwnPropertyNames(model),
        },
      })
      .afterClosed()
      .subscribe(() => {
        this.loadSalary();
        this.loadSalaries();
      });
  }

  clearFile() {
    this.fileInput.nativeElement.value = '';
  }
}
