import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { SelfServiceOvertimeService } from 'src/app/self-service/services/self-service-overtime.service';
import { Observable } from 'rxjs';
import { PaginatedList } from 'src/app/core/shared/paginated-list';
import { MatDialog } from '@angular/material/dialog';
import { SelfserveOvertimeComponent } from 'src/app/self-service/pages/selfserve-overtime/selfserve-overtime.component';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-self-service-overtimes',
  templateUrl: './self-service-overtimes.component.html',
  styleUrls: ['./self-service-overtimes.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class SelfServiceOvertimesComponent implements OnInit {
  readonly displayedColumns: string[] = ['date', 'time', 'status', 'actions'];

  pageIndex: number = 0;

  pageSize: number = 20;

  totalCount: number = 0;

  dataSource: MatTableDataSource<Overtime>;

  constructor(
    private overtimeService: SelfServiceOvertimeService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadOvertimes();
  }

  create(): void {
    this.dialog.open(SelfserveOvertimeComponent);
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
  }

  private loadOvertimes(): void {
    this.overtimeService.list().subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
      this.totalCount = data.totalCount;
    });
  }
}
