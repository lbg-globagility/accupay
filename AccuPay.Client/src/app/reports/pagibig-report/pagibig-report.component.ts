import { Component, ViewChild } from '@angular/core';
import { SelectMonthComponent } from '../components/select-month/select-month.component';
import { Moment } from 'moment';
import { ReportService } from '../report.service';
import { ErrorHandler } from '../../core/shared/services/error-handler';

@Component({
  selector: 'app-pagibig-report',
  templateUrl: './pagibig-report.component.html',
  styleUrls: ['./pagibig-report.component.scss'],
})
export class PagibigReportComponent {
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
      .getPagIBIGReport(month, year)
      .catch((err) => {
        this.errorHandler.badRequest(err, 'Error downloading PAGIBIG Report.');
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
