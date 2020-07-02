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

    console.log(this.monthForm.date.value);
    console.log(new Date(this.monthForm.date.value));

    var date = this.monthForm.date.value as Moment;
    var month = Number(date.format('M'));
    var year = date.year();
    console.log(month);
    console.log(date.year());

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
