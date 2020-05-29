import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Division } from '../shared/division';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';
import { DivisionService } from '../division.service';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-division-list',
  templateUrl: './division-list.component.html',
  styleUrls: ['./division-list.component.scss']
})
export class DivisionListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'parentName',
    'name',
    'divisionType'
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  divisions: Division[];

  totalCount: number;

  dataSource: MatTableDataSource<Division>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'name',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private divisionService: DivisionService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getDivisionList());
  }

  ngOnInit(): void {
    this.getDivisionList();
  }

  getDivisionList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.divisionService.getAll(options, this.searchTerm).subscribe((data) => {
      this.divisions = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.divisions);
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
    this.getDivisionList();
  }
}
