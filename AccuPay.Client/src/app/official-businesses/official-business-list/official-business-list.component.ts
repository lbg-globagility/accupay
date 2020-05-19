import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';

@Component({
  selector: 'app-official-business-list',
  templateUrl: './official-business-list.component.html',
  styleUrls: ['./official-business-list.component.scss']
})
export class OfficialBusinessListComponent implements OnInit {

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

  officialBusinesses: OfficialBusiness[];

  totalCount: number;

  dataSource: MatTableDataSource<OfficialBusiness>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(private officialBusinessService: OfficialBusinessService) { 
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getOfficialBusinessList());
  }

  ngOnInit(): void {
    this.getOfficialBusinessList();
  }

  getOfficialBusinessList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.officialBusinessService
      .getAll(options, this.searchTerm)
      .subscribe((data) => {
        this.officialBusinesses = data.items;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(this.officialBusinesses);
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
    this.getOfficialBusinessList();
  }

}
