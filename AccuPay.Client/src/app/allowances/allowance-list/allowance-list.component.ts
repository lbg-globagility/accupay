import { auditTime, filter } from 'rxjs/operators';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Allowance } from 'src/app/allowances/shared/allowance';
import { AllowanceService } from 'src/app/allowances/allowance.service';
import { Router } from '@angular/router';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';
import { AllowanceImportParserOutput } from '../shared/allowance-import-parser-output';
import { MatDialog } from '@angular/material/dialog';
import { PostImportParserOutputDialogComponent } from 'src/app/shared/import/post-import-parser-output-dialog/post-import-parser-output-dialog.component';
import { ImportParserOutput } from 'src/app/shared/import/import-parser-output';
import { AllowanceImportModel } from '../shared/allowance-import-model';

@Component({
  selector: 'app-allowance-list',
  templateUrl: './allowance-list.component.html',
  styleUrls: ['./allowance-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class AllowanceListComponent implements OnInit {
  @ViewChild('uploader') fileInput: ElementRef;

  readonly displayedColumns: string[] = [
    'employeeNumber',
    'employeeName',
    'allowanceType',
    'frequency',
    'date',
    'amount',
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  allowances: Allowance[];

  totalCount: number;

  dataSource: MatTableDataSource<Allowance>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;
  isDownloadingTemplate: boolean;

  constructor(
    private allowanceService: AllowanceService,
    private router: Router,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getAllowanceList());
  }

  ngOnInit(): void {
    this.getAllowanceList();
  }

  getAllowanceList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.allowanceService.getAll(options, this.searchTerm).subscribe((data) => {
      this.allowances = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.allowances);
    });
  }

  gotoNewAllowance(): void {
    this.router.navigate(['/allowances/new']);
  }

  applyFilter(searchTerm: string): void {
    this.searchTerm = searchTerm;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox(): void {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
  }

  sortData(sort: Sort): void {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number): void {
    this.selectedRow = id;
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getAllowanceList();
  }

  downloadTemplate(): void {
    this.isDownloadingTemplate = true;
    this.allowanceService
      .getAllowanceTemplate()
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading allowance template.'
        );
      })
      .finally(() => {
        this.isDownloadingTemplate = false;
      });
  }

  onImport(files: FileList) {
    const file = files[0];

    this.allowanceService.import(file).subscribe(
      (outputParse) => {
        this.displaySuccess(outputParse);
        this.clearFile();
      },
      (err) => {
        this.errorHandler.badRequest(err, 'Failed to import allowance.');
        this.clearFile();
      }
    );
  }

  private displaySuccess(outputParse: AllowanceImportParserOutput) {
    const model: AllowanceImportModel = {
      employeeNo: '',
      allowanceName: '',
      effectiveStartDate: new Date(),
      effectiveEndDate: new Date(),
      allowanceFrequency: '',
      amount: 0,
      remarks: '',
    };

    this.dialog
      .open(PostImportParserOutputDialogComponent, {
        data: {
          columnHeaders: new AllowanceImportParserOutput().columnHeaders,
          invalidRecords: outputParse.invalidRecords,
          validRecords: outputParse.validRecords,
          propertyNames: Object.getOwnPropertyNames(model),
        },
      })
      .afterClosed()
      .subscribe(() => this.getAllowanceList());
  }

  clearFile() {
    this.fileInput.nativeElement.value = '';
  }
}
