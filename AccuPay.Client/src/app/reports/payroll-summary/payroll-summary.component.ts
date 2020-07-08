import { Component } from '@angular/core';
import { ReportService } from '../report.service';
import { ErrorHandler } from '../../core/shared/services/error-handler';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { SelectPayperiodDialogComponent } from '../components/select-payperiod-dialog/select-payperiod-dialog.component';

@Component({
  selector: 'app-payroll-summary',
  templateUrl: './payroll-summary.component.html',
  styleUrls: ['./payroll-summary.component.scss'],
})
export class PayrollSummaryComponent {
  isDownloading: boolean = false;

  form: FormGroup = this.fb.group({
    salaryDistribution: ['All', Validators.required],
    hideEmptyColumns: [true, Validators.required],
  });

  constructor(
    private fb: FormBuilder,
    private reportService: ReportService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {}

  generateReport(): void {
    const dialogRef = this.dialog.open(SelectPayperiodDialogComponent, {
      minWidth: '500px',
      data: {
        title: 'Delete Leave',
        content: 'Are you sure you want to delete this leave?',
      },
    });

    // if (!this.monthForm.date.valid) {
    //   return;
    // }
    // var date = this.monthForm.date.value as Moment;
    // var month = Number(date.format('M'));
    // var year = date.year();
    // let payPeriodFromId = 629;
    // let payPeriodToId = 629;
    // this.isDownloading = true;
    // this.reportService
    //   .getPayrollSummary(payPeriodFromId, payPeriodToId)
    //   .catch((err) => {
    //     this.errorHandler.badRequest(err, 'Error downloading Payroll Summary.');
    //   })
    //   .finally(() => {
    //     this.isDownloading = false;
    //   });
  }
}
