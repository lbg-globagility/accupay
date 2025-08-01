import { Component, OnInit } from '@angular/core';
import { ReportService } from '../report.service';
import { ErrorHandler } from '../../core/shared/services/error-handler';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { SelectPayperiodRangeDialogComponent } from '../components/select-payperiod-range-dialog/select-payperiod-range-dialog.component';

@Component({
  selector: 'app-payroll-summary',
  templateUrl: './payroll-summary.component.html',
  styleUrls: ['./payroll-summary.component.scss'],
})
export class PayrollSummaryComponent implements OnInit {
  isDownloading: boolean = false;

  form: FormGroup = this.fb.group({
    salaryDistribution: ['All', Validators.required],
    hideEmptyColumns: [true, Validators.required],
    keepInOneSheet: [true, Validators.required],
  });

  constructor(
    private fb: FormBuilder,
    private reportService: ReportService,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    // only one checkbox is checked even if we add default values
    // on the FormGroup so we need to manually set the value here
    this.form.get('hideEmptyColumns').setValue(true);
    this.form.get('keepInOneSheet').setValue(true);
  }

  generateReport(): void {
    if (!this.form.valid) {
      return;
    }

    const dialogRef = this.dialog.open(SelectPayperiodRangeDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (!result || !result.selectedFrom || !result.selectedTo) {
        return;
      }

      let payPeriodFromId = result.selectedFrom.id;
      let payPeriodToId = result.selectedTo.id;
      let keepInOneSheet = this.form.get('keepInOneSheet').value;
      let hideEmptyColumns = this.form.get('hideEmptyColumns').value;
      let salaryDistribution = this.form.get('salaryDistribution').value;
      this.isDownloading = true;

      this.reportService
        .getPayrollSummary(
          payPeriodFromId,
          payPeriodToId,
          keepInOneSheet,
          hideEmptyColumns,
          salaryDistribution
        )
        .catch((err) => {
          console.log(err);
          this.errorHandler.badRequest(
            err,
            'Error downloading Payroll Summary.'
          );
        })
        .finally(() => {
          this.isDownloading = false;
        });
    });
  }
}
