import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Employee } from 'src/app/employees/shared/employee';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Router } from '@angular/router';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { EmployeePageOptions } from 'src/app/employees/shared/employee-page-options';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss'],
  host: {
    class: 'h-full',
  },
})
export class EmployeesComponent implements OnInit {
  @ViewChild('employeesRef')
  employeesRef: ElementRef;

  searchTerm: string;

  filter: string = 'Active only';

  pageIndex = 0;

  pageSize = 25;

  employees: Employee[];

  totalCount: number;

  modelChanged: Subject<void>;

  selectedEmployees: Employee[];

  selectedEmployee: Employee;

  constructor(
    private employeeService: EmployeeService,
    private router: Router
  ) {
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

  selectEmployee(): void {
    const employee =
      this.selectedEmployees.length > 0 ? this.selectedEmployees[0] : null;

    this.router.navigate(['employees', employee.id]);
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
      this.resetScroll();

      this.selectedEmployee = this.employees[0];
      this.selectedEmployees = [this.selectedEmployee];
      this.router.navigate(['employees', this.selectedEmployee.id]);
    });
  }

  private resetScroll(): void {
    this.employeesRef.nativeElement.scrollTo(0, 0);
  }
}
