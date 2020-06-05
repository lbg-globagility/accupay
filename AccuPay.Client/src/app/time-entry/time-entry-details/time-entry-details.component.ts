import { Component, OnInit } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { TimeEntryService } from '../time-entry.service';
import { LoadingState } from 'src/app/core/states/loading-state';
import { Sort } from '@angular/material/sort';
import { auditTime } from 'rxjs/operators';
import { Employee } from '../shared/employee';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-time-entry-details',
  templateUrl: './time-entry-details.component.html',
  styleUrls: ['./time-entry-details.component.scss'],
})
export class TimeEntryDetailsComponent implements OnInit {
  payPeriodId = Number(this.route.snapshot.paramMap.get('id'));

  readonly displayedColumns: string[] = [
    'employeeNo',
    'fullName',
    'regularHours',
    'absentHours',
    'lateHours',
    'undertimeHours',
    'leaveHours',
    'overtimeHours',
    'nightDifferentialHours',
    'nightDifferentialOvertimeHours',
  ];

  payPeriod: PayPeriod;

  savingState: LoadingState = new LoadingState();

  sort: Sort = {
    active: 'employeeName',
    direction: '',
  };

  dataSource: MatTableDataSource<Employee>;
  modelChanged: Subject<any>;
  employees: Employee[];

  pageIndex: number = 0;
  pageSize: number = 10;
  totalPages: number;
  totalCount: number;
  term: string;

  selectedRow: number;

  constructor(
    private payPeriodService: PayPeriodService,
    private timeEntryService: TimeEntryService,
    private route: ActivatedRoute
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadEmployees());
  }

  ngOnInit(): void {
    this.loadPayPeriod();
    this.loadEmployees();
  }

  private loadPayPeriod(): void {
    this.payPeriodService.getById(this.payPeriodId).subscribe((data) => {
      this.payPeriod = data;
    });
  }

  loadEmployees(): void {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.timeEntryService
      .getEmployees(this.payPeriodId, options, this.term)
      .subscribe(async (data) => {
        await setTimeout(() => {
          this.employees = data.items;

          this.totalPages = data.totalPages;
          this.totalCount = data.totalCount;

          this.dataSource = new MatTableDataSource(this.employees);
        });
      });
  }

  afterGenerate() {
    this.modelChanged.next();
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
    this.loadEmployees();
  }
}
