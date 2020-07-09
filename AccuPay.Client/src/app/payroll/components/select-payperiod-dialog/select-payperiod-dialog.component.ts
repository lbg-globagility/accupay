import { Component, OnInit } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { MatDialogRef } from '@angular/material/dialog';
import { SelectionModel } from '@angular/cdk/collections';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { findIndex } from 'lodash';
import { flatMap, tap } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-select-payperiod-dialog',
  templateUrl: './select-payperiod-dialog.component.html',
  styleUrls: ['./select-payperiod-dialog.component.scss'],
})
export class SelectPayperiodDialogComponent implements OnInit {
  readonly displayedColumns = ['isSelected', 'dateFrom', 'dateTo', 'status'];

  payPeriodDataSource: PayPeriod[];
  selection: SelectionModel<PayPeriod>;

  previousYear: number;
  nextYear: number;

  latestPayperiod: PayPeriod;

  constructor(
    private payPeriodService: PayPeriodService,
    private dialogRef: MatDialogRef<SelectPayperiodDialogComponent>,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadData();
  }

  loadData(): void {
    let year = new Date().getFullYear();
    this.payPeriodService
      .getLatest()
      .pipe(
        tap((payPeriod) => {
          this.latestPayperiod = payPeriod;
        }),
        flatMap(() => this.payPeriodService.getYearlyPayPeriods(year))
      )
      .subscribe((data) => {
        this.updatePreviousAndNextYear(year);

        this.focusPayPeriodAfterLatestPayPeriod(data);
      });
  }

  loadList(year: number): void {
    this.updatePreviousAndNextYear(year);

    this.payPeriodService.getYearlyPayPeriods(year).subscribe((data) => {
      this.payPeriodDataSource = data;
      this.selection.clear();
    });
  }

  private focusPayPeriodAfterLatestPayPeriod(data: PayPeriod[]) {
    let latestPayPeriodId = this.latestPayperiod.id;

    let selectedIndex = findIndex(data, function (payPeriod) {
      return payPeriod.id == latestPayPeriodId;
    });

    let selectedPayPeriod: PayPeriod;

    if (selectedIndex >= 0) {
      // select the next pay period
      selectedIndex++;
      selectedPayPeriod = data[selectedIndex];

      if (selectedPayPeriod != null && selectedPayPeriod.status != 'Pending') {
        selectedPayPeriod = null;
      }

      let selector = this.getFocusedQuerySelector(selectedIndex);
      setTimeout(() => {
        document.querySelector(selector).scrollIntoView();
      });
    }

    this.selection = new SelectionModel<PayPeriod>(false, [selectedPayPeriod]);
    this.payPeriodDataSource = data;
  }

  private updatePreviousAndNextYear(year: number) {
    this.previousYear = year - 1;
    this.nextYear = year + 1;
  }

  private getFocusedQuerySelector(selectedIndex: number) {
    if (selectedIndex <= 5) {
      return '#mat-table';
    }

    let focusedIndex = selectedIndex - 5;

    if (focusedIndex) return `.row${this.payPeriodDataSource[focusedIndex].id}`;
  }

  startPayroll(): void {
    if (this.selection?.selected.length < 1) {
      this.showWarningSnackBar('Please select a pay period.');
      return;
    }

    let selectedPayPeriod = this.selection.selected[0];

    if (!selectedPayPeriod) {
      this.showWarningSnackBar('Please select a pay period.');
      return;
    }

    if (selectedPayPeriod.status != 'Pending') {
      this.showWarningSnackBar("You can only start 'Pending' pay periods.");
      return;
    }

    this.dialogRef.close(this.selection.selected[0]);
  }

  private showWarningSnackBar(message: string) {
    this.snackBar.open(message, null, {
      duration: 5000,
      panelClass: ['mat-toolbar', 'mat-accent'],
    });
  }
}
