import { Component, OnInit } from '@angular/core';
import { TimeLog } from '../shared/time-log';
import { BehaviorSubject } from 'rxjs';
import { TimeLogService } from '../time-log.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-view-time-log',
  templateUrl: './view-time-log.component.html',
  styleUrls: ['./view-time-log.component.scss']
})
export class ViewTimeLogComponent implements OnInit {
  timeLog: TimeLog;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  timeLogId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private timeLogService: TimeLogService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTimeLog();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete time-log',
        content: 'Are you sure you want to delete this time-log?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.timeLogService.delete(this.timeLogId).subscribe(() => {
        this.router.navigate(['time-logs']);
        Swal.fire({
          title: 'Deleted',
          text: `The time-log was successfully deleted.`,
          icon: 'success',
          showConfirmButton: true,
        });
      });
    });
  }

  private loadTimeLog(): void {
    this.timeLogService.get(this.timeLogId).subscribe((data) => {
      this.timeLog = data;

      this.isLoading.next(true);
    });
  }
}
