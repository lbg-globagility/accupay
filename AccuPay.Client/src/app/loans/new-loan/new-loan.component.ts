import Swal from 'sweetalert2';
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { LoanService } from 'src/app/loans/loan.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { LoanFormComponent } from 'src/app/loans/loan-form/loan-form.component';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-new-loan',
  templateUrl: './new-loan.component.html',
  styleUrls: ['./new-loan.component.scss'],
  host: {
    class: 'block',
  },
})
export class NewLoanComponent {
  @ViewChild(LoanFormComponent)
  form: LoanFormComponent;

  constructor(
    private loanService: LoanService,
    private router: Router,
    private errorHandler: ErrorHandler,
    private dialog: MatDialogRef<NewLoanComponent>
  ) {}

  save(): void {
    if (!this.form.valid) {
      return;
    }

    const loan = this.form.value;

    this.loanService.create(loan).subscribe(
      (result) => {
        this.displaySuccess();
        this.dialog.close(result);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create loan.')
    );
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new loan!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
