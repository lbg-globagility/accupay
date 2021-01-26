import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanService } from 'src/app/loans/loan.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { PermissionTypes } from 'src/app/core/auth';

@Component({
  selector: 'app-view-loan',
  templateUrl: './view-loan.component.html',
  styleUrls: ['./view-loan.component.scss'],
})
export class ViewLoanComponent implements OnInit {
  readonly PermissionTypes = PermissionTypes;

  loan: Loan;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  loanId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private loanService: LoanService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadLoan();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Loan',
        content: 'Are you sure you want to delete this loan?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.loanService.delete(this.loanId).subscribe(
        () => {
          this.router.navigate(['loans']);
          Swal.fire({
            title: 'Deleted',
            text: `The loan was successfully deleted.`,
            icon: 'success',
            showConfirmButton: true,
          });
        },
        (err) => this.errorHandler.badRequest(err, 'Failed to delete loan.')
      );
    });
  }

  private loadLoan(): void {
    this.loanService.get(this.loanId).subscribe((data) => {
      this.loan = data;

      this.isLoading.next(true);
    });
  }
}
