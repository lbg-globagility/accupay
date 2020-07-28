import { auditTime, filter } from 'rxjs/operators';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Loan } from 'src/app/loans/shared/loan';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { LoanService } from 'src/app/loans/loan.service';
import { MatDialog } from '@angular/material/dialog';
import { LoanHistoryComponent } from '../loan-history/loan-history.component';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { LoanImportParserOutput } from '../shared/loam-import-parser-output';
import { PermissionTypes } from 'src/app/core/auth';

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

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  loans: Loan[];

  totalCount: number;

  dataSource: MatTableDataSource<Loan>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'startDate',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;
  isDownloadingTemplate: boolean;

  constructor(
    private loanService: LoanService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler,
    private router: Router
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getLoanList());
  }

  ngOnInit(): void {
    this.getLoanList();
  }

  getLoanList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.loanService.getAll(options, this.searchTerm).subscribe((data) => {
      this.loans = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.loans);
    });
  }

  applyFilter(searchTerm: string) {
    this.searchTerm = searchTerm;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number) {
    this.selectedRow = id;
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getLoanList();
  }

  viewLoan(loan: Loan) {
    this.router.navigate(['loans', loan.id]);
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
      .subscribe(() => this.getLoanList());
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

  routeToNewLoan() {
    this.router.navigate(['loans', 'new']);
    // routerLink="loans/new"
  }

  onImport(files: FileList) {
    const file = files[0];

    this.loanService.import(file).subscribe(
      (outputParse) => {
        this.getLoanList();
        this.displaySuccess(outputParse);
        this.clearFile();
      },
      (err) => {
        this.errorHandler.badRequest(err, 'Failed to import shift.');
        this.clearFile();
      }
    );
  }

  private displaySuccess(outputParse: LoanImportParserOutput) {
    const hasFailedImports =
      outputParse.invalidRecords && outputParse.invalidRecords.length > 0;
    const succeeds =
      outputParse.validRecords && outputParse.validRecords.length > 0;

    if (!hasFailedImports && succeeds) {
      Swal.fire({
        title: 'Success',
        text: 'Successfully imported new loan(s)!',
        icon: 'success',
        timer: 3000,
        showConfirmButton: false,
      });
    } else if (hasFailedImports && !succeeds) {
      Swal.fire({
        title: 'Failed',
        text: `${outputParse.invalidRecords.length} loan(s) failed to import.`,
        icon: 'error',
        showConfirmButton: true,
      });
    } else if (hasFailedImports && succeeds) {
      Swal.fire({
        title: 'Oops!',
        text: `${outputParse.invalidRecords.length} loan(s) were failed to import
        and the ${outputParse.validRecords.length} loan(s) succeeded.`,
        icon: 'warning',
        showConfirmButton: true,
      });
    }
  }

  clearFile() {
    this.fileInput.nativeElement.value = '';
  }
}
