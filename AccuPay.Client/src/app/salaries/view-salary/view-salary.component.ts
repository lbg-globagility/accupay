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

@Component({
  selector: 'app-view-salary',
  templateUrl: './view-salary.component.html',
  styleUrls: ['./view-salary.component.scss'],
})
export class ViewSalaryComponent implements OnInit {
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
    this.salaryService.getLatest(this.employeeId).subscribe((salary) => {
      this.latestSalary = salary;

      this.isLoading.next(true);
    });
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
        this.loadSalary();
        this.loadSalaries();

        this.displaySuccess(outputParse);
        this.clearFile();
      },
      (err) => {
        this.errorHandler.badRequest(err, 'Failed to import shift.');
        this.clearFile();
      }
    );
  }

  private displaySuccess(outputParse: SalaryImportParserOutput) {
    const hasFailedImports =
      outputParse.invalidRecords && outputParse.invalidRecords.length > 0;
    const succeeds =
      outputParse.validRecords && outputParse.validRecords.length > 0;

    const manySucceed = succeeds ? outputParse.validRecords.length > 1 : false;
    const manyFailed = hasFailedImports
      ? outputParse.invalidRecords.length > 1
      : false;

    if (!hasFailedImports && succeeds) {
      Swal.fire({
        title: 'Success',
        text: `Successfully imported new ${
          manySucceed ? 'salaries' : 'salary'
        }!`,
        icon: 'success',
        timer: 3000,
        showConfirmButton: false,
      });
    } else if (hasFailedImports && !succeeds) {
      Swal.fire({
        title: 'Failed',
        text: `${outputParse.invalidRecords.length} ${
          manyFailed ? 'salaries' : 'salary'
        } failed to import.`,
        icon: 'error',
        showConfirmButton: true,
      });
    } else if (hasFailedImports && succeeds) {
      Swal.fire({
        title: 'Oops!',
        text: `${outputParse.invalidRecords.length} ${
          manyFailed ? 'salaries' : 'salary'
        } were failed to import
        and the ${outputParse.validRecords.length} ${
          manySucceed ? 'salaries' : 'salary'
        } succeeded.`,
        icon: 'warning',
        showConfirmButton: true,
      });
    }
  }

  clearFile() {
    this.fileInput.nativeElement.value = '';
  }
}
