import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Position } from 'src/app/positions/shared/position';
import { PositionService } from 'src/app/positions/position.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-view-position',
  templateUrl: './view-position.component.html',
  styleUrls: ['./view-position.component.scss'],
})
export class ViewPositionComponent implements OnInit {
  position: Position;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  positionId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private positionService: PositionService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadPosition();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Position',
        content: 'Are you sure you want to delete this position?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.positionService.delete(this.positionId).subscribe(
        () => {
          this.router.navigate(['positions']);
          Swal.fire({
            title: 'Deleted',
            text: `The position was successfully deleted.`,
            icon: 'success',
            showConfirmButton: true,
          });
        },
        (err) => this.errorHandler.badRequest(err, 'Failed to delete position.')
      );
    });
  }

  private loadPosition(): void {
    this.positionService.get(this.positionId).subscribe((data) => {
      this.position = data;

      this.isLoading.next(true);
    });
  }
}
