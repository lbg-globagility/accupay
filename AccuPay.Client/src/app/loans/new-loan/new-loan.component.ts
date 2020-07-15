import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanService } from 'src/app/loans/loan.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-loan',
  templateUrl: './new-loan.component.html',
  styleUrls: ['./new-loan.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewLoanComponent {
  constructor(
    private loanService: LoanService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(loan: Loan): void {
    this.loanService.create(loan).subscribe(
      (result) => {
        this.displaySuccess();
        this.router.navigate(['loans', result.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create loan.')
    );
  }

  onCancel(): void {
    this.router.navigate(['loans']);
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
