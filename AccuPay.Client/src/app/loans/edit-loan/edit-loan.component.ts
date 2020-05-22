import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanService } from 'src/app/loans/loan.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-loan',
  templateUrl: './edit-loan.component.html',
  styleUrls: ['./edit-loan.component.scss'],
})
export class EditLoanComponent implements OnInit {
  loan: Loan;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  loanId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private loanService: LoanService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadLoan();
  }

  onSave(loan: Loan): void {
    this.loanService.update(loan, this.loanId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['loans', this.loanId]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update loan.')
    );
  }

  onCancel(): void {
    this.router.navigate(['loans', this.loanId]);
  }

  private loadLoan(): void {
    this.loanService.get(this.loanId).subscribe((data) => {
      this.loan = data;

      this.isLoading.next(true);
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
