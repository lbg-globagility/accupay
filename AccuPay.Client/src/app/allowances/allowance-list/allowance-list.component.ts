import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-allowance-list',
  templateUrl: './allowance-list.component.html',
  styleUrls: ['./allowance-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class AllowanceListComponent implements OnInit {
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
    private errorHandler: ErrorHandler
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
}
