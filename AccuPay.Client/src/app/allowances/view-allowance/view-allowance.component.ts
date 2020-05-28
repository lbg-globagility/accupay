import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Allowance } from 'src/app/allowances/shared/allowance';
import { AllowanceService } from 'src/app/allowances/allowance.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-view-allowance',
  templateUrl: './view-allowance.component.html',
  styleUrls: ['./view-allowance.component.scss'],
})
export class ViewAllowanceComponent implements OnInit {
  allowance: Allowance;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  allowanceId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private allowanceService: AllowanceService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadAllowance();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Allowance',
        content: 'Are you sure you want to delete this allowance?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.allowanceService.delete(this.allowanceId).subscribe(
        () => {
          this.router.navigate(['allowances']);
          Swal.fire({
            title: 'Deleted',
            text: `The allowance was successfully deleted.`,
            icon: 'success',
            showConfirmButton: true,
          });
        },
        (err) =>
          this.errorHandler.badRequest(err, 'Failed to delete allowance.')
      );
    });
  }

  private loadAllowance(): void {
    this.allowanceService.get(this.allowanceId).subscribe((data) => {
      this.allowance = data;

      this.isLoading.next(true);
    });
  }
}
