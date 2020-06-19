import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';

@Component({
  selector: 'app-salary-list',
  templateUrl: './salary-list.component.html',
  styleUrls: ['./salary-list.component.scss'],
})
export class SalaryListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employeeNumber',
    'employeeName',
    'employeeType',
    'effectiveFrom',
    'basicSalary',
    'allowanceSalary',
    'totalSalary',
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  salaries: Salary[];

  totalCount: number;

  dataSource: MatTableDataSource<Salary>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'effectiveFrom',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private salaryService: SalaryService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getSalaryList());
  }

  ngOnInit(): void {
    this.getSalaryList();
  }

  getSalaryList(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.salaryService.list(options, this.searchTerm).subscribe((data) => {
      this.salaries = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.salaries);
    });
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
    this.getSalaryList();
  }
}
