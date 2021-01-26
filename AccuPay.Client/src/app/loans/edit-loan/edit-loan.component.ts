import Swal from 'sweetalert2';
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanService } from 'src/app/loans/loan.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LoanFormComponent } from 'src/app/loans/loan-form/loan-form.component';

@Component({
  selector: 'app-edit-loan',
  templateUrl: './edit-loan.component.html',
  styleUrls: ['./edit-loan.component.scss'],
})
export class EditLoanComponent implements OnInit {
  @ViewChild(LoanFormComponent)
  form: LoanFormComponent;

  loan: Loan;

  constructor(
    private loanService: LoanService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<EditLoanComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.loan = data.loan;
  }

  ngOnInit(): void {}

  save(): void {
    const loan = this.form.value;

    this.loanService.update(loan, this.loan.id).subscribe({
      next: (result) => {
        this.displaySuccess();
        this.dialog.close(result);
      },
      error: (err) =>
        this.errorHandler.badRequest(err, 'Failed to update loan.'),
    });
  }

  private displaySuccess(): void {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
