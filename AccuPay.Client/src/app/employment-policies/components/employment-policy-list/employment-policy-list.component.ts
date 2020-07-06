import { Component, OnInit } from '@angular/core';
import { EmploymentPolicyService } from 'src/app/employment-policies/services/employment-policy.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialog } from '@angular/material/dialog';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { Subject } from 'rxjs';
import { EmploymentPolicy } from 'src/app/employment-policies/shared/employment-policy';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';

@Component({
  selector: 'app-employment-policy-list',
  templateUrl: './employment-policy-list.component.html',
  styleUrls: ['./employment-policy-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EmploymentPolicyListComponent implements OnInit {
  readonly displayedColumns: string[] = ['name', 'actions'];

  searchTerm: string;

  modelChanged: Subject<any>;

  totalCount: number;

  dataSource: MatTableDataSource<EmploymentPolicy>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  constructor(
    private employmentPolicyService: EmploymentPolicyService,
    private dialog: MatDialog,
    private errorHandler: ErrorHandler
  ) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.loadEmploymentPolicies());
  }

  ngOnInit(): void {
    this.loadEmploymentPolicies();
  }

  clearSearchBox(): void {
    this.searchTerm = '';
    this.applyFilter();
  }

  applyFilter(): void {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  sortData(sort: Sort): void {
    this.sort = sort;
    this.modelChanged.next();
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadEmploymentPolicies();
  }

  private loadEmploymentPolicies() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.employmentPolicyService.list(options).subscribe((data) => {
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(data.items);
    });
  }
}
