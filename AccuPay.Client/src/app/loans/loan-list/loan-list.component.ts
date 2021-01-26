import { auditTime, filter } from 'rxjs/operators';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Loan } from 'src/app/loans/shared/loan';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { LoanService } from 'src/app/loans/loan.service';
import { MatDialog } from '@angular/material/dialog';
import { LoanHistoryComponent } from '../loan-history/loan-history.component';
import { Router } from '@angular/router';
import { LoanImportParserOutput } from '../shared/loan-import-parser-output';
import { LoanImportModel } from '../shared/loan-import-model';
import { PostImportParserOutputDialogComponent } from 'src/app/shared/import/post-import-parser-output-dialog/post-import-parser-output-dialog.component';
import { PermissionTypes } from 'src/app/core/auth';
import { LoanTypeService } from 'src/app/loan-types/service/loan-type.service';
import { LoanType } from 'src/app/loan-types/shared/loan-type';
import { LoanPageOptions } from 'src/app/loans/shared/loan-page-options';
import { NewLoanComponent } from 'src/app/loans/new-loan/new-loan.component';
import { EditLoanComponent } from 'src/app/loans/edit-loan/edit-loan.component';
import { ViewLoanComponent } from 'src/app/loans/view-loan/view-loan.component';
import { ViewLoanDialogComponent } from 'src/app/loans/view-loan-dialog/view-loan-dialog.component';

@Component({
  selector: 'app-loan-list',
  templateUrl: './loan-list.component.html',
  styleUrls: ['./loan-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class LoanListComponent implements OnInit {
  readonly PermissionTypes = PermissionTypes;

  @ViewChild('uploader') fileInput: ElementRef;

  readonly displayedColumns: string[] = [
    'employee',
    'loanType',
    'startDate',
    'deductionSchedule',
    'totalLoanAmount',
    'totalBalanceLeft',
    'status',
    'actions',
  ];

  searchTerm: string;

  loanTypeId: number;

  status: string;

  modelChanged: Subject<any>;

  loans: Loan[];

  loanTypes: LoanType[] = [];

  statusTypes: string[];

  totalCount: number;

  dataSource: MatTableDataSource<Loan>;

  pageIndex: number = 0;

  pageSize: number = 10;

  isDownloadingTemplate: boolean;

  constructor(
    private loanService: LoanService,
    private loanTypeService: LoanTypeService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler,
    private router: Router
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadLoans());
  }

  ngOnInit(): void {
    this.loadLoans();
    this.loadLoanTypes();
    this.loadStatusTypes();
  }

  applyFilter() {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.searchTerm = '';
    this.applyFilter();
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadLoans();
  }

  viewLoan(loan: Loan) {
    // this.router.navigate(['loans', loan.id]);
    this.dialog
      .open(ViewLoanDialogComponent, {
        data: { loan },
      })
      .afterClosed();
  }

  viewHistory(loan: Loan) {
    this.dialog
      .open(LoanHistoryComponent, {
        data: {
          loanId: loan.id,
        },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadLoans());
  }

  downloadTemplate(): void {
    this.isDownloadingTemplate = true;
    this.loanService
      .getLoanTemplate()
      .catch((err) => {
        this.errorHandler.badRequest(err, 'Error downloading loan template.');
      })
      .finally(() => {
        this.isDownloadingTemplate = false;
      });
  }

  newLoan(): void {
    this.dialog
      .open(NewLoanComponent)
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadLoans());
  }

  editLoan(loan: Loan): void {
    this.dialog
      .open(EditLoanComponent, {
        data: { loan },
      })
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadLoans());
  }

  onImport(files: FileList) {
    const file = files[0];

    this.loanService.import(file).subscribe(
      (outputParse) => {
        this.loadLoans();
        this.displaySuccess(outputParse);
        this.clearFile();
      },
      (err) => {
        this.errorHandler.badRequest(err, 'Failed to import shift.');
        this.clearFile();
      }
    );
  }

  private loadLoans(): void {
    const options = new LoanPageOptions(
      this.pageIndex,
      this.pageSize,
      this.searchTerm,
      this.loanTypeId,
      this.status
    );

    this.loanService.list(options).subscribe((data) => {
      this.loans = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.loans);
    });
  }

  private loadLoanTypes(): void {
    this.loanTypeService
      .getAll()
      .subscribe((loanTypes) => (this.loanTypes = loanTypes));
  }

  private loadStatusTypes(): void {
    this.loanService
      .getStatusList()
      .subscribe((statusTypes) => (this.statusTypes = statusTypes));
  }

  private displaySuccess(outputParse: LoanImportParserOutput) {
    const model: LoanImportModel = {
      employeeNo: '',
      employeeName: '',
      loanName: '',
      loanNumber: '',
      startDate: new Date(),
      totalLoanAmount: 0,
      totalLoanBalance: 0,
      deductionAmount: 0,
      deductionSchedule: '',
      comments: '',
      remarks: '',
    };

    this.dialog
      .open(PostImportParserOutputDialogComponent, {
        data: {
          columnHeaders: new LoanImportParserOutput().columnHeaders,
          invalidRecords: outputParse.invalidRecords,
          validRecords: outputParse.validRecords,
          propertyNames: Object.getOwnPropertyNames(model),
        },
      })
      .afterClosed()
      .subscribe(() => this.loadLoans());
  }

  clearFile() {
    this.fileInput.nativeElement.value = '';
  }
}
