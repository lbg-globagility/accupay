import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PayPeriod } from '../../shared/payperiod';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TimeEntryService } from '../../time-entry.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { LoadingState } from 'src/app/core/states/loading-state';

@Component({
  selector: 'app-time-entry-summary-details',
  templateUrl: './time-entry-summary-details.component.html',
  styleUrls: ['./time-entry-summary-details.component.scss'],
})
export class TimeEntrySummaryDetailsComponent implements OnInit {
  @Input()
  payPeriodId: number;

  @Input()
  canGenerate: Boolean;

  @Output()
  afterGenerate: EventEmitter<PayPeriod> = new EventEmitter();

  @Input()
  panelOpenState: Boolean;

  loadingState: LoadingState = new LoadingState();

  payPeriod: PayPeriod;

  constructor(
    private snackBar: MatSnackBar,
    private timeEntryService: TimeEntryService,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadDetails();
  }

  loadDetails(): void {
    this.loadingState.changeToLoading();

    this.timeEntryService
      .getDetails(this.payPeriodId)
      .subscribe((payPeriod) => {
        this.payPeriod = payPeriod;
        this.loadingState.changeToSuccess();
      });
  }

  onGenerate(): void {
    this.snackBar.open('Generating time entries');

    this.timeEntryService.generate(this.payPeriodId).subscribe({
      next: (result) => {
        this.snackBar.open('Finished generating time entries', 'OK');
        this.afterGenerate.emit();
      },
      error: (err) => {
        this.errorHandler.badRequest(err, 'Failed to generate time entries');
      },
    });
  }
}
