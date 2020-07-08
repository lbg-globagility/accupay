import { Component, OnInit } from '@angular/core';
import { PayPeriodService } from 'src/app/payroll/services/payperiod.service';
import { MatDialogRef } from '@angular/material/dialog';
import { SelectionModel } from '@angular/cdk/collections';
import { DatePipe } from '@angular/common';
import { PayPeriod } from 'src/app/payroll/shared/payperiod';

@Component({
  selector: 'app-select-payperiod-dialog',
  templateUrl: './select-payperiod-dialog.component.html',
  styleUrls: ['./select-payperiod-dialog.component.scss'],
  providers: [DatePipe],
})
export class SelectPayperiodDialogComponent implements OnInit {
  readonly displayedColumns = ['isSelected', 'dateFrom', 'dateTo', 'status'];

  payPeriodDataSource: PayPeriod[];
  selection = new SelectionModel<PayPeriod>(true, []);

  previousYear: number;
  nextYear: number;

  constructor(
    private payperiodService: PayPeriodService,
    private dialogRef: MatDialogRef<SelectPayperiodDialogComponent>,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    var year = new Date().getFullYear();
    this.loadList(year);
  }

  loadList(year: number): void {
    this.previousYear = year - 1;
    this.nextYear = year + 1;

    this.payperiodService.getYearlyPayPeriods(year).subscribe((data) => {
      this.payPeriodDataSource = data;
      this.selection.clear();
      this.updateTitle();
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  selectedDates: string;

  updateTitle(): void {
    if (this.selection.selected.length > 2) {
      let firstSelection = this.selection.selected[0];
      this.selection.deselect(firstSelection);
    }

    let selectedFrom, selectedTo: PayPeriod;

    ({ selectedFrom, selectedTo } = this.GetSelectedPayPeriods());

    if (selectedFrom == null || selectedTo == null) {
      this.selectedDates = null;
    } else {
      this.selectedDates =
        this.datePipe.transform(selectedFrom.cutoffStart) +
        ' - ' +
        this.datePipe.transform(selectedTo.cutoffEnd);
    }
  }

  download(): void {
    console.log(this.GetSelectedPayPeriods());
  }

  private GetSelectedPayPeriods() {
    let selectedFrom, selectedTo: PayPeriod;

    if (this.selection.selected.length == 1) {
      selectedFrom = this.selection.selected[0];
      selectedTo = this.selection.selected[0];
    } else if (this.selection.selected.length == 2) {
      if (
        this.selection.selected[0].cutoffStart <=
        this.selection.selected[1].cutoffStart
      ) {
        selectedFrom = this.selection.selected[0];
        selectedTo = this.selection.selected[1];
      } else {
        selectedFrom = this.selection.selected[1];
        selectedTo = this.selection.selected[0];
      }
    }
    return { selectedFrom, selectedTo };
  }
}
