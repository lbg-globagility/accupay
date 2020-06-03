import { Component, OnInit } from '@angular/core';
import { PayperiodService } from 'src/app/payroll/services/payperiod.service';
import { Payperiod } from 'src/app/payroll/shared/payperiod';
import { TimeEntryService } from '../time-entry.service';
import { LoadingState } from 'src/app/core/states/loading-state';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-time-entry',
  templateUrl: './time-entry.component.html',
  styleUrls: ['./time-entry.component.scss'],
})
export class TimeEntryComponent implements OnInit {
  payPeriods: Payperiod[];

  selectedPayPeriod: Payperiod;

  savingState: LoadingState = new LoadingState();

  constructor(
    private payperiodService: PayperiodService,
    private timeEntryService: TimeEntryService,
    private errorHandler: ErrorHandler,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadPayPeriods();
  }

  loadPayPeriods(): void {
    this.payperiodService.list().subscribe((data) => {
      this.payPeriods = data.items;
    });
  }

  generateTimeEntries() {
    const payperiodId = this.selectedPayPeriod?.id;

    if (!payperiodId) {
      this.snackBar.open('Please select a payperiod first.', null, {
        duration: 5000,
        panelClass: ['mat-toolbar', 'mat-warn'],
      });

      return;
    }

    console.log(payperiodId);

    this.snackBar.open('Generating time entries');

    this.timeEntryService.generate(payperiodId).subscribe({
      next: (result) => {
        this.snackBar.open('Finished generating time entries.', 'OK');
      },
      error: (err) => {
        this.errorHandler.badRequest(err, 'Failed to generate time entries');
      },
    });
  }
}
