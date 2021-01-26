import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeUserService } from '../../unregistered-employees/employee-user.service';
import { LoadingState } from 'src/app/core/states/loading-state';
import { EmployeePageOptions } from 'src/app/employees/shared/employee-page-options';
import { PageEvent } from '@angular/material/paginator';
import { remove } from 'lodash';

@Component({
  selector: 'app-unregistered-employee-list',
  templateUrl: './unregistered-employee-list.component.html',
  styleUrls: ['./unregistered-employee-list.component.scss'],
})
export class UnregisteredEmployeeListComponent implements OnInit {
  employees: Employee[];

  searchTerm: string = null;

  loadingState: LoadingState = new LoadingState();

  selectedEmployees: Employee[];
  selectedEmployees2: Employee[];

  modelChanged: Subject<any>;
  pageIndex: number = 0;
  pageSize: number = 25;
  filter: string = 'Active only';
  totalCount: number;

  selectedCount: number = 0;

  isMultiple = false;

  constructor(
    private service: EmployeeUserService,
    private dialogRef: MatDialogRef<UnregisteredEmployeeListComponent>
  ) {
    this.dialogRef.disableClose = true;
    this.loadingState.changeToLoading();

    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getEmployees(this.searchTerm));
  }

  ngOnInit(): void {
    this.getEmployees();

    this.selectedEmployees2 = [];
  }

  getEmployees(searchTerm: string = null) {
    const options = new EmployeePageOptions(
      this.pageIndex,
      this.pageSize,
      searchTerm,
      this.filter
    );

    this.service.getEmployees(options).subscribe((e) => {
      this.loadingState.changeToSuccess();

      this.employees = e.items;
      this.totalCount = e.totalCount;
    });
  }

  applyFilter(): void {
    this.loadingState.changeToLoading();
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.loadingState.changeToLoading();
    this.pageIndex = 0;
    this.searchTerm = null;
    this.modelChanged.next();
  }

  page(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.modelChanged.next();
  }

  selectionChange(event$: any) {
    if (!this.isMultiple) {
      this.selectedEmployees2 = this.selectedEmployees;
      this.selectedCount = this.selectedEmployees2.length;
      return;
    }

    this.multipleSelection(event$);
  }

  private multipleSelection(event$: any) {
    const eventOption = event$.option;
    const unselected = eventOption._selected === false;
    if (unselected) {
      remove(this.selectedEmployees2, (e) => {
        return e.id === +eventOption._value.id;
      });
    }

    this.selectedEmployees2.push(...this.selectedEmployees);

    const updatedSelectedEmployees = Array.from(
      new Set(this.selectedEmployees2.map((e) => e.id))
    ).map((id) => {
      const employee = this.selectedEmployees2.find((e) => e.id === id);
      const newEmployeeUser = {
        id,
        lastName: employee.lastName,
        firstName: employee.firstName,
        emailAddress: employee.emailAddress,
      };

      return newEmployeeUser as Employee;
    });

    this.selectedEmployees2 = updatedSelectedEmployees;

    this.selectedCount = updatedSelectedEmployees.length;
  }

  clickOK() {
    this.dialogRef.close(this.selectedEmployees2);
  }

  clickCancel() {
    this.dialogRef.close();
  }
}
