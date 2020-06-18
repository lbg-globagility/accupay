import { Component, OnInit, Inject } from '@angular/core';
import { PaystubService } from '../../services/paystub.service';
import { Paystub } from '../../shared/paystub';
import { Adjustment } from '../../shared/adjustment';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-adjustments-breakdown',
  templateUrl: './adjustments-breakdown.component.html',
  styleUrls: ['./adjustments-breakdown.component.scss'],
})
export class AdjustmentsBreakdownComponent implements OnInit {
  readonly displayedColumns = ['description', 'amount'];

  dataSource: Adjustment[];

  paystub: Paystub;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(
    private paystubService: PaystubService,
    private dialogRef: MatDialogRef<AdjustmentsBreakdownComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.paystub = data.paystub;
  }

  ngOnInit(): void {
    this.loadAdjustments();
  }

  private loadAdjustments(): void {
    this.paystubService.GetAdjustments(this.paystub.id).subscribe((data) => {
      this.dataSource = data;

      this.isLoading.next(true);
    });
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
