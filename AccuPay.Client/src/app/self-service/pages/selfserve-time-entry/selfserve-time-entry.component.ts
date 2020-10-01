import { Component, OnInit } from '@angular/core';
import { TimeEntry } from 'src/app/time-entry/shared/time-entry';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { SelfserveService } from '../../services/selfserve.service';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { LoadingState } from 'src/app/core/states/loading-state';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { PayperiodSelectComponent } from '../../components/payperiod-select/payperiod-select.component';

@Component({
  selector: 'app-selfserve-time-entry',
  templateUrl: './selfserve-time-entry.component.html',
  styleUrls: ['./selfserve-time-entry.component.scss'],
})
export class SelfserveTimeEntryComponent implements OnInit {
  timeEntries: TimeEntry[];

  startingPayrollState: LoadingState = new LoadingState();
  latestPayPeriod: any;

  constructor(
    private service: SelfserveService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog,
    private dialogTimesheet: MatDialogRef<SelfserveTimeEntryComponent>
  ) {}

  ngOnInit(): void {
    this.getTimeEntries(null);
  }

  private getTimeEntries(payPeriodId?: number) {
    this.service
      .getEmployeeTimeEntryByPeriod(payPeriodId)
      .subscribe((t) => (this.timeEntries = t));
  }

  selectedPeriodChanged(payPeriodId: number) {
    this.getTimeEntries(payPeriodId);
  }

  selectPeriod(): void {
    this.dialog
      .open(PayperiodSelectComponent, {
        minWidth: '500px',
      })
      .afterClosed()
      .subscribe(
        (payPeriod) => {
          const selectedPeriod = payPeriod as PayPeriod;

          this.getTimeEntries(selectedPeriod.id);
        },
        (err) => {
          this.startingPayrollState.changeToNothing();
        }
      );
  }
}
