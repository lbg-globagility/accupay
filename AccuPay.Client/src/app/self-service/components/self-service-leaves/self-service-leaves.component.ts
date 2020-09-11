import { Component, OnInit } from '@angular/core';
import { SelfServiceLeaveService } from 'src/app/self-service/services';
import { MatTableDataSource } from '@angular/material/table';
import { Leave } from 'src/app/leaves/shared/leave';
import { MatDialog } from '@angular/material/dialog';
import { SelfserveLeaveComponent } from 'src/app/self-service/pages/selfserve-leave/selfserve-leave.component';

@Component({
  selector: 'app-self-service-leaves',
  templateUrl: './self-service-leaves.component.html',
  styleUrls: ['./self-service-leaves.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class SelfServiceLeavesComponent implements OnInit {
  readonly displayedColumns: string[] = ['type', 'date', 'time', 'status'];

  dataSource: MatTableDataSource<Leave>;

  constructor(
    private leaveService: SelfServiceLeaveService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadLeaves();
  }

  private loadLeaves(): void {
    this.leaveService.list().subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
    });
  }

  createLeave(): void {
    this.dialog.open(SelfserveLeaveComponent).afterClosed().subscribe();
  }
}
