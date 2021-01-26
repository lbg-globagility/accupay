import { Component, ViewChild } from '@angular/core';
import { ReportService } from '../report.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Moment } from 'moment';
import { SelectMonthComponent } from '../components/select-month/select-month.component';

@Component({
  selector: 'app-tax-report',
  templateUrl: './tax-report.component.html',
  styleUrls: ['./tax-report.component.scss'],
})
export class TaxReportComponent {
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

    let date = this.monthForm.date.value as Moment;
    let month = Number(date.format('M'));
    let year = date.year();

    this.isDownloading = true;
    this.reportService
      .getTaxReport(month, year)
      .catch((err) => {
        this.errorHandler.badRequest(err, 'Error downloading Tax Report.');
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
