import { Component, ViewChild } from '@angular/core';
import { SelectDateRangeComponent } from '../components/select-date-range/select-date-range.component';
import { ReportService } from '../report.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Moment } from 'moment';

@Component({
  selector: 'app-thirteenth-month-report',
  templateUrl: './thirteenth-month-report.component.html',
  styleUrls: ['./thirteenth-month-report.component.scss'],
})
export class ThirteenthMonthReportComponent {
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

    let dateFrom = (this.dateRangeForm.form.controls.startDate
      .value as Moment).format('MM-DD-yyyy');

    let dateTo = (this.dateRangeForm.form.controls.endDate
      .value as Moment).format('MM-DD-yyyy');

    this.isDownloading = true;
    this.reportService
      .get13thMonthReport(dateFrom, dateTo)
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading 13th Month Report.'
        );
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
