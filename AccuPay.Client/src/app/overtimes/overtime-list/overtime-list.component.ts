import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';

@Component({
  selector: 'app-overtime-list',
  templateUrl: './overtime-list.component.html',
  styleUrls: ['./overtime-list.component.scss']
})
export class OvertimeListComponent implements OnInit {

  readonly displayedColumns: string[] = [
    'employeeNumber',
    'employeeName',
    'date',
    'time',
    'status',
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  overtimes: Overtime[];

  totalCount: number;

  dataSource: MatTableDataSource<Overtime>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'firstName',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private overtimeService: OvertimeService) { 
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getOvertimeList());
  }

  ngOnInit(): void {
    this.getOvertimeList();
  }

  getOvertimeList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.overtimeService
      .getAll(options, this.searchTerm)
      .subscribe((data) => {
        this.overtimes = data.items;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(this.overtimes);
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
    this.getOvertimeList();
  }

}
