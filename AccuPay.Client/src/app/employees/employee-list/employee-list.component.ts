import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employee.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Employee } from '../shared/employee';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { Sort } from '@angular/material/sort';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit {
  readonly displayedColumns: string[] = ['employeeNo', 'lastName', 'firstName'];

  totalPages: number;
  totalCount: number;
  term: string;
  employees: Employee[];

  placeholder: string;

  dataSource: MatTableDataSource<Employee>;

  modelChanged: Subject<any>;

  pageIndex = 0;

  pageSize = 10;

  organizations: string[] = [];

  selectedOrganization = '';

  sort: Sort = {
    active: 'lastName',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private employeeService: EmployeeService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.load());
  }

  ngOnInit(): void {
    this.load();
  }

  load() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.employeeService.getList(options, this.term).subscribe(async (data) => {
      await setTimeout(() => {
        this.employees = data.items;

        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;

        this.dataSource = new MatTableDataSource(this.employees);
      });
    });
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.load();
  }

  clearSearchBox() {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
  }

  applyFilter(term: string) {
    this.term = term;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number) {
    this.selectedRow = id;
  }
}
