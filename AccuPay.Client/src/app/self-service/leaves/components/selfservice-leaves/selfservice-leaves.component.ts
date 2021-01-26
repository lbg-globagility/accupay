import { Component, OnInit } from '@angular/core';
import { SelfServiceLeaveService } from 'src/app/self-service/services';
import { MatTableDataSource } from '@angular/material/table';
import { Leave } from 'src/app/leaves/shared/leave';
import { MatDialog } from '@angular/material/dialog';
import { SelfserviceNewLeaveComponent } from 'src/app/self-service/leaves/components/selfservice-new-leave/selfservice-new-leave.component';

@Component({
  selector: 'app-selfservice-leaves',
  templateUrl: './selfservice-leaves.component.html',
  styleUrls: ['./selfservice-leaves.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class SelfserviceLeavesComponent implements OnInit {
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
    this.dialog.open(SelfserviceNewLeaveComponent).afterClosed().subscribe();
  }
}
