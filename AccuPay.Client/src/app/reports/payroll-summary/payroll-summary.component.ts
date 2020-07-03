import { Component, ViewChild } from '@angular/core';
import { SelectMonthComponent } from '../components/select-month/select-month.component';
import { Moment } from 'moment';
import { ReportService } from '../report.service';
import { ErrorHandler } from '../../core/shared/services/error-handler';

@Component({
  selector: 'app-payroll-summary',
  templateUrl: './payroll-summary.component.html',
  styleUrls: ['./payroll-summary.component.scss'],
})
export class PayrollSummaryComponent {
  @ViewChild(SelectMonthComponent)
  monthForm: SelectMonthComponent;

  isDownloading: boolean = false;

  constructor(
    private reportService: ReportService,
    private errorHandler: ErrorHandler
  ) {}

  generateReport(): void {
    if (!this.monthForm.date.valid) {
      return;
    }

    // var date = this.monthForm.date.value as Moment;
    // var month = Number(date.format('M'));
    // var year = date.year();

    let payPeriodFromId = 629;
    let payPeriodToId = 629;

    this.isDownloading = true;
    this.reportService
      .getPayrollSummary(payPeriodFromId, payPeriodToId)
      .catch((err) => {
        this.errorHandler.badRequest(err, 'Error downloading Payroll Summary.');
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
