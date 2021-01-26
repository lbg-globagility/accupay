import { Component, OnInit, ViewChild } from '@angular/core';
import { SelectMonthComponent } from '../components/select-month/select-month.component';
import { ReportService } from '../report.service';
import { Moment } from 'moment';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-philhealth-report',
  templateUrl: './philhealth-report.component.html',
  styleUrls: ['./philhealth-report.component.scss'],
})
export class PhilhealthReportComponent {
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
      .getPhilHealthReport(month, year)
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading PhilHealth Report.'
        );
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
