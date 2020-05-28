import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Position } from 'src/app/positions/shared/position';
import { PositionService } from 'src/app/positions/position.service';

@Component({
  selector: 'app-position-list',
  templateUrl: './position-list.component.html',
  styleUrls: ['./position-list.component.scss'],
})
export class PositionListComponent implements OnInit {
  readonly displayedColumns: string[] = ['divisionName', 'name'];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  positions: Position[];

  totalCount: number;

  dataSource: MatTableDataSource<Position>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'name',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private positionService: PositionService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getPositionList());
  }

  ngOnInit(): void {
    this.getPositionList();
  }

  getPositionList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.positionService.getAll(options, this.searchTerm).subscribe((data) => {
      this.positions = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.positions);
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
    this.getPositionList();
  }
}
