import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';

@Component({
  selector: 'app-view-leave',
  templateUrl: './view-leave.component.html',
  styleUrls: ['./view-leave.component.scss'],
})
export class ViewLeaveComponent implements OnInit {
  leave: Leave;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  leaveId = this.route.snapshot.paramMap.get('id');

  constructor(
    private leaveService: LeaveService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadLeave();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Leave',
        content: 'Are you sure you want to delete this leave?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.leaveService.delete(this.leaveId).subscribe(() => {
        this.router.navigate(['leaves']);
        Swal.fire({
          title: 'Deleted',
          text: `The leave was successfully deleted.`,
          icon: 'success',
          showConfirmButton: true,
        });
      });
    });
  }

  private loadLeave(): void {
    this.leaveService.get(this.leaveId).subscribe((data) => {
      this.leave = data;

      this.isLoading.next(true);
    });
  }
}
