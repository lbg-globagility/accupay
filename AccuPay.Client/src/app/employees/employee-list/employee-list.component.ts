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
import { EmployeePageOptions } from 'src/app/employees/shared/employee-page-options';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EmployeeListComponent implements OnInit {
  readonly displayedColumns: string[] = ['employeeNo', 'lastName', 'firstName'];

  dataSource: MatTableDataSource<Employee>;

  totalCount: number;

  searchTerm: string;

  modelChanged: Subject<any>;

  pageIndex = 0;

  pageSize = 10;

  sort: Sort = {
    active: 'lastName',
    direction: '',
  };

  constructor(private employeeService: EmployeeService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadEmployees());
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  clearSearchBox() {
    this.searchTerm = '';
    this.applyFilter();
  }

  applyFilter() {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadEmployees();
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  private loadEmployees() {
    const options = new EmployeePageOptions(
      this.pageIndex,
      this.pageSize,
      this.searchTerm
    );

    this.employeeService.list(options).subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalCount = data.totalCount;
    });
  }
}
