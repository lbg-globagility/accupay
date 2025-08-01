import { Component, ViewChild } from '@angular/core';
import { SelectMonthComponent } from '../components/select-month/select-month.component';
import { Moment } from 'moment';
import { ReportService } from '../report.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-sss-report',
  templateUrl: './sss-report.component.html',
  styleUrls: ['./sss-report.component.scss'],
})
export class SssReportComponent {
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
      .getSSSReport(month, year)
      .catch((err) => {
        this.errorHandler.badRequest(err, 'Error downloading SSS Report.');
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
