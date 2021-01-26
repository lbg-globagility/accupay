import { Component, ViewChild } from '@angular/core';
import { Moment } from 'moment';
import { ReportService } from '../report.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { FormGroup, FormBuilder } from '@angular/forms';
import { SelectDateRangeComponent } from '../components/select-date-range/select-date-range.component';

@Component({
  selector: 'app-loan-report-bytype',
  templateUrl: './loan-report-bytype.component.html',
  styleUrls: ['./loan-report-bytype.component.scss'],
})
export class LoanReportBytypeComponent {
  @ViewChild(SelectDateRangeComponent)
  dateRangeForm: SelectDateRangeComponent;

  form: FormGroup = this.fb.group({
    isPerPage: [null],
  });

  isDownloading: boolean = false;

  constructor(
    private fb: FormBuilder,
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

    let isPerPage = this.form.controls.isPerPage.value;
    if (isPerPage == null) {
      isPerPage = false;
    }

    this.isDownloading = true;
    this.reportService
      .getLoanByTypeReport(dateFrom, dateTo, isPerPage)
      .catch((err) => {
        this.errorHandler.badRequest(
          err,
          'Error downloading Loan By Type Report.'
        );
      })
      .finally(() => {
        this.isDownloading = false;
      });
  }
}
