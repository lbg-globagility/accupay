import { Component, Input } from '@angular/core';
import { Paystub } from '../../shared/paystub';
import { MatDialog } from '@angular/material/dialog';
import { AdjustmentsBreakdownComponent } from '../adjustments-breakdown/adjustments-breakdown.component';
import { LoansBreakdownComponent } from '../loans-breakdown/loans-breakdown.component';

@Component({
  selector: 'app-paystub-details',
  templateUrl: './paystub-details.component.html',
  styleUrls: ['./paystub-details.component.scss'],
})
export class PaystubDetailsComponent {
  @Input()
  paystub: Paystub;

  constructor(private dialog: MatDialog) {}

  showAdjustmentsBreakdown(): void {
    this.dialog.open(AdjustmentsBreakdownComponent, {
      data: {
        paystub: this.paystub,
      },
    });
  }

  showLoansBreakdown(): void {
    this.dialog.open(LoansBreakdownComponent, {
      data: {
        paystub: this.paystub,
      },
    });
  }
}
