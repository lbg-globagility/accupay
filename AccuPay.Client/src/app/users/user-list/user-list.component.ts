import { auditTime } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/core/shared/constants';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { User } from 'src/app/users/shared/user';
import { UserService } from 'src/app/users/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'image',
    'firstName',
    'lastName',
    'email',
  ];

  placeholder: string;

  users: User[];

  dataSource: MatTableDataSource<User>;

  modelChanged: Subject<any>;

  totalCount: number;

  pageIndex = 0;

  pageSize: number = 10;

  term: string;

  organizations: string[] = [];

  selectedOrganization = '';

  sort: Sort = {
    active: 'firstName',
    direction: '',
  };

  maxRowCount: number = 10;

  clearSearch = '';

  selectedRow: number;

  constructor(private userService: UserService) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getUserList());
  }

  ngOnInit() {
    this.getUserList();
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getUserList();
  }

  clearSearchBox() {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
  }

  getUserList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.userService
      .getByOrganization(options, this.term, this.selectedOrganization)
      .subscribe((data) => {
        this.users = data.items;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(this.users);
      });
  }

  applyFilter(term: string) {
    this.term = term;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  applyOrganizationName() {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number) {
    this.selectedRow = id;
  }
}
