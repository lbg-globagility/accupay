import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Organization } from 'src/app/organizations/shared/organization';
import { OrganizationService } from 'src/app/organizations/organization.service';
import { PageEvent } from '@angular/material/paginator';
import { PageOptions } from 'src/app/core/shared/page-options';

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class OrganizationListComponent implements OnInit {
  readonly displayedColumns: string[] = ['name'];

  pageIndex = 1;

  pageSize = 10;

  totalCount = 0;

  dataSource: MatTableDataSource<Organization>;

  constructor(private organizationService: OrganizationService) {}

  ngOnInit(): void {
    this.loadOrganizations();
  }

  loadOrganizations() {
    const options = new PageOptions(this.pageIndex, this.pageSize);

    this.organizationService.list(options).subscribe((data) => {
      this.dataSource = new MatTableDataSource<Organization>(data.items);
      this.totalCount = data.totalCount;
    });
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadOrganizations();
  }
}
