import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Employee } from 'src/app/employees/shared/employee';
import { PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { EmployeePageOptions } from 'src/app/employees/shared/employee-page-options';

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

  filter: string = 'Active only';

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

  applyFilter(): void {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox(): void {
    this.searchTerm = null;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  page(pageEvent: PageEvent): void {
    console.log(pageEvent);
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadEmployees();
  }

  private loadEmployees(): void {
    const options = new EmployeePageOptions(
      this.pageIndex,
      this.pageSize,
      this.searchTerm,
      this.filter
    );

    this.employeeService.list(options).subscribe((data) => {
      this.employees = data.items;
      this.totalCount = data.totalCount;
    });
  }
}
