import { Component, ViewChild } from '@angular/core';
import { SelectDateRangeComponent } from '../components/select-date-range/select-date-range.component';
import { ReportService } from '../report.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Moment } from 'moment';

@Component({
  selector: 'app-loan-report-byemployee',
  templateUrl: './loan-report-byemployee.component.html',
  styleUrls: ['./loan-report-byemployee.component.scss'],
})
export class LoanReportByemployeeComponent {
  @ViewChild(SelectDateRangeComponent)
  dateRangeForm: SelectDateRangeComponent;

  isDownloading: boolean = false;

  constructor(
    private reportService: ReportService,
    private errorHandler: ErrorHandler
  ) {}

  generateReport(): void {
    if (!this.dateRangeForm.form.valid) {
      return;
    }

    var dateFrom = this.dateRangeForm.form.controls.startDate.value as Moment;
    var monthFrom = Number(dateFrom.format('M'));
    var dayFrom = dateFrom.date();
    var yearFrom = dateFrom.year();

    var dateTo = this.dateRangeForm.form.controls.endDate.value as Moment;
    var monthTo = Number(dateTo.format('M'));
    var dayTo = dateTo.date();
    var yearTo = dateTo.year();

    this.isDownloading = true;
    this.reportService
      .getLoanByEmployeeReport(
        monthFrom,
        dayFrom,
        yearFrom,
        monthTo,
        dayTo,
        yearTo
      )
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading Loan By Employee Report.'
        );
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
