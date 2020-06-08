import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { LeaveLedger } from '../shared/leave-ledger';
import { LeaveService } from '../leave.service';
import { PageOptions } from 'src/app/core/shared/page-options';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-leave-ledger',
  templateUrl: './leave-ledger.component.html',
  styleUrls: ['./leave-ledger.component.scss']
})
export class LeaveLedgerComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'date',
    'type',
    'amount',
    'balance',
  ];

  employeeId: number;

  type: string;

  totalCount: number;

  dataSource: MatTableDataSource<LeaveLedger>;

  searchTerm: string;

  pageIndex = 0;

  pageSize = 10;

  sort: Sort = {
    active: 'date',
    direction: '',
  };

  constructor(    
    private leaveService: LeaveService,
    @Inject(MAT_DIALOG_DATA) private data: any)
    {
    this.employeeId = data.employeeId;
    this.type = data.type;
  }

  ngOnInit(): void {
    this.getLedger()
  }

  onPageChanged(pageEvent: PageEvent): void {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getLedger();
  }

  getLedger(){
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.leaveService.getLedger(options,this.employeeId, this.type).subscribe((data) => {
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(data.items);
    });
  }

}
