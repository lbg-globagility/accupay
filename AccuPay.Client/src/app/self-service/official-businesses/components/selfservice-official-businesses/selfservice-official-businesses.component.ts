import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { PageOptions } from 'src/app/core/shared/page-options';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { SelfserviceNewOfficialBusinessComponent } from 'src/app/self-service/official-businesses/components/selfservice-new-official-business/selfservice-new-official-business.component';
import { SelfserviceOfficialBusinessService } from 'src/app/self-service/services/selfservice-official-business.service';

@Component({
  selector: 'app-selfservice-official-businesses',
  templateUrl: './selfservice-official-businesses.component.html',
  styleUrls: ['./selfservice-official-businesses.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class SelfserviceOfficialBusinessesComponent implements OnInit {
  readonly displayedColumns: string[] = ['date', 'time', 'status'];

  pageIndex: number = 0;

  pageSize: number = 10;

  totalCount: number = 0;

  dataSource: MatTableDataSource<OfficialBusiness>;

  constructor(
    private officialBusinessService: SelfserviceOfficialBusinessService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadOfficialBusinesses();
  }

  create(): void {
    this.dialog.open(SelfserviceNewOfficialBusinessComponent);
  }

  page(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.loadOfficialBusinesses();
  }

  private loadOfficialBusinesses(): void {
    const options = new PageOptions(this.pageIndex, this.pageSize);

    this.officialBusinessService.list(options).subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalCount = data.totalCount;
    });
  }
}
