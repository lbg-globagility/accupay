import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { Role } from 'src/app/roles/shared/role';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';
import { PageOptions } from 'src/app/core/shared/page-options';
import { RoleService } from 'src/app/roles/services/role.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss'],
})
export class RoleListComponent implements OnInit {
  readonly displayedColumns: string[] = ['name', 'actions'];

  searchTerm: string;

  modelChanged: Subject<any>;

  totalCount: number;

  dataSource: MatTableDataSource<Role>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  constructor(private roleService: RoleService) {}

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.roleService.list(options).subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalCount = data.totalCount;
    });
  }

  applyFilter() {
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.searchTerm = '';
    this.applyFilter();
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadRoles();
  }
}
