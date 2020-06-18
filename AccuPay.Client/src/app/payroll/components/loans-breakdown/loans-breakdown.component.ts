import { Component, OnInit, Inject } from '@angular/core';
import { PaystubService } from '../../services/paystub.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Paystub } from '../../shared/paystub';
import { BehaviorSubject } from 'rxjs';
import { LoanTransaction } from '../../shared/loan-transaction';

@Component({
  selector: 'app-loans-breakdown',
  templateUrl: './loans-breakdown.component.html',
  styleUrls: ['./loans-breakdown.component.scss'],
})
export class LoansBreakdownComponent implements OnInit {
  readonly displayedColumns = [
    'loanNumber',
    'loanType',
    'totalAmount',
    'deductionAmount',
    'balance',
  ];

  dataSource: LoanTransaction[];

  paystub: Paystub;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(
    private paystubService: PaystubService,
    private dialogRef: MatDialogRef<LoansBreakdownComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.paystub = data.paystub;
  }

  ngOnInit(): void {
    this.loadLoanTypes();
  }

  private loadLoanTypes(): void {
    this.paystubService
      .getLoanTransactions(this.paystub.id)
      .subscribe((data) => {
        this.dataSource = data;

        this.isLoading.next(true);
      });
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
