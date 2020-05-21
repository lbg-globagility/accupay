import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-view-overtime',
  templateUrl: './view-overtime.component.html',
  styleUrls: ['./view-overtime.component.scss'],
})
export class ViewOvertimeComponent implements OnInit {
  overtime: Overtime;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  overtimeId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private overtimeService: OvertimeService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadOvertime();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Overtime',
        content: 'Are you sure you want to delete this overtime?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.overtimeService.delete(this.overtimeId).subscribe(
        () => {
          this.router.navigate(['overtimes']);
          Swal.fire({
            title: 'Deleted',
            text: `The overtime was successfully deleted.`,
            icon: 'success',
            showConfirmButton: true,
          });
        },
        (err) => this.errorHandler.badRequest(err, 'Failed to delete overtime.')
      );
    });
  }

  private loadOvertime(): void {
    this.overtimeService.get(this.overtimeId).subscribe((data) => {
      this.overtime = data;

      this.isLoading.next(true);
    });
  }
}
