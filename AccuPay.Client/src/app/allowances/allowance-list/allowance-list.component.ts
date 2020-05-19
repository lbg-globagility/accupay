import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Allowance } from 'src/app/allowances/shared/allowance';
import { AllowanceService } from 'src/app/allowances/allowance.service';

@Component({
  selector: 'app-allowance-list',
  templateUrl: './allowance-list.component.html',
  styleUrls: ['./allowance-list.component.scss']
})
export class AllowanceListComponent implements OnInit {

  readonly displayedColumns: string[] = [
    'employeeNumber',
    'employeeName',
    'allowanceType',
    'frequency',
    'date',
    'amount',
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  allowances: Allowance[];

  totalCount: number;

  dataSource: MatTableDataSource<Allowance>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private allowanceService: AllowanceService) { 
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getAllowanceList());
  }

  ngOnInit(): void {
    this.getAllowanceList();
  }

  getAllowanceList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.allowanceService
      .getAll(options, this.searchTerm)
      .subscribe((data) => {
        this.allowances = data.items;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(this.allowances);
      });
  }

  applyFilter(searchTerm: string) {
    this.searchTerm = searchTerm;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
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
    this.getAllowanceList();
  }

}
