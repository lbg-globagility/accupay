import { Component, OnInit } from '@angular/core';
import { SelfserveService } from '../../services/selfserve.service';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';
import { tap, flatMap } from 'rxjs/operators';
import { findIndex } from 'lodash';
import { SelectionModel } from '@angular/cdk/collections';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-payperiod-select',
  templateUrl: './payperiod-select.component.html',
  styleUrls: ['./payperiod-select.component.scss'],
})
export class PayperiodSelectComponent implements OnInit {
  readonly displayedColumns = ['isSelected', 'dateFrom', 'dateTo'];
  payperiods: PayPeriod[];
  previousYear: number;
  nextYear: number;
  latestPayperiod: PayPeriod;
  selection: SelectionModel<PayPeriod>;

  constructor(
    private service: SelfserveService,
    private dialogRef: MatDialogRef<PayperiodSelectComponent>
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    const year = new Date().getFullYear();
    this.service
      .getLatest()
      .pipe(
        tap((payPeriod) => {
          this.latestPayperiod = payPeriod;
        }),
        flatMap(() => this.service.getPayperiodsByYear(year))
      )
      .subscribe((data) => {
        this.payperiods = data;

        this.updatePreviousAndNextYear(year);

        this.focusPayPeriodAfterLatestPayPeriod(data);
      });
  }

  getPayperiodsByYear(year: number) {
    this.service
      .getPayperiodsByYear(year)
      .subscribe((payperiods) => (this.payperiods = payperiods));
  }

  private updatePreviousAndNextYear(year: number) {
    this.previousYear = year - 1;
    this.nextYear = year + 1;
  }

  private focusPayPeriodAfterLatestPayPeriod(data: PayPeriod[]) {
    const latestPayPeriodId = this.latestPayperiod.id;

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
  }

  private getFocusedQuerySelector(selectedIndex: number) {
    if (selectedIndex <= 5) {
      return '#mat-table';
    }

    const focusedIndex = selectedIndex - 5;

    if (focusedIndex) {
      return `.row${this.payperiods[focusedIndex].id}`;
    }
  }

  selectionChanged() {
    this.dialogRef.close(this.selection.selected[0]);
  }
}
