import { Component, OnInit } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { TimeEntryService } from '../time-entry.service';
import { LoadingState } from 'src/app/core/states/loading-state';
import { Sort } from '@angular/material/sort';
import { auditTime } from 'rxjs/operators';
import { Employee } from '../shared/employee';
import { MatTableDataSource } from '@angular/material/table';
import { Subject, BehaviorSubject } from 'rxjs';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { TimeEntry } from 'src/app/time-entry/shared/time-entry';

@Component({
  selector: 'app-time-entry-details',
  templateUrl: './time-entry-details.component.html',
  styleUrls: ['./time-entry-details.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class TimeEntryDetailsComponent implements OnInit {
  payPeriodId = Number(this.route.snapshot.paramMap.get('id'));

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  readonly displayedColumns: string[] = [
    'employee',
    'regularHours',
    'leaveHours',
    'overtimeHours',
    'nightDifferentialHours',
    'nightDifferentialOvertimeHours',
    'lateHours',
    'undertimeHours',
    'absentHours',
  ];

  payPeriod: PayPeriod;

  savingState: LoadingState = new LoadingState();

  dataSource: MatTableDataSource<Employee>;

  totalCount: number;

  sort: Sort = {
    active: 'employee',
    direction: '',
  };

  pageIndex: number = 0;
  pageSize: number = 10;

  searchTerm: string;

  timeEntries: TimeEntry[];

  expandedEmployee: Employee;

  modelChanged: Subject<any>;

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

      this.isLoading.next(true);
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
      .getEmployees(this.payPeriodId, options, this.searchTerm)
      .subscribe((data) => {
        this.dataSource = new MatTableDataSource(data.items);
        console.log(this.dataSource);
        this.totalCount = data.totalCount;
      });
  }

  loadTimeEntries(): void {
    if (this.expandedEmployee == null) {
      return;
    }

    this.timeEntryService
      .getTimeEntries(this.payPeriodId, this.expandedEmployee.id)
      .subscribe((timeEntries) => {
        this.timeEntries = timeEntries;
      });
  }

  afterGenerate(): void {
    this.modelChanged.next();
  }

  sortData(sort: Sort): void {
    this.sort = sort;
    this.modelChanged.next();
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.modelChanged.next();
  }

  toggleExpansion(employee: Employee): void {
    this.expandedEmployee =
      this.expandedEmployee === employee ? null : employee;

    this.loadTimeEntries();
  }
}
