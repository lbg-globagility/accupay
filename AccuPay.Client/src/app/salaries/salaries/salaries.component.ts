import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Employee } from 'src/app/employees/shared/employee';
import { PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';

@Component({
  selector: 'app-salaries',
  templateUrl: './salaries.component.html',
  styleUrls: ['./salaries.component.scss'],
  host: {
    class: 'h-full',
  },
})
export class SalariesComponent implements OnInit {
  searchTerm: string;

  pageIndex = 0;

  pageSize = 25;

  employees: Employee[];

  totalCount: number;

  modelChanged: Subject<void>;

  constructor(private employeeService: EmployeeService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadEmployees());
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  searchBoxChange(): void {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox(): void {
    this.searchTerm = null;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  page(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadEmployees();
  }

  private loadEmployees(): void {
    const options = new PageOptions(this.pageIndex, this.pageSize);

    this.employeeService.list(options, this.searchTerm).subscribe((data) => {
      this.employees = data.items;
      this.totalCount = data.totalCount;
    });
  }
}
