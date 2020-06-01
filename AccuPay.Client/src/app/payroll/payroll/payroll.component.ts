import { Component, OnInit } from '@angular/core';
import { Payperiod } from 'src/app/payroll/shared/payperiod';
import { PayperiodService } from 'src/app/payroll/services/payperiod.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { StartPayrollDialogComponent } from 'src/app/payroll/start-payroll-dialog/start-payroll-dialog.component';
import { filter, map, mergeMap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
})
export class PayrollComponent implements OnInit {
  latestPayperiod: Payperiod;

  readonly displayedColumns = ['cutoff', 'status'];

  dataSource: MatTableDataSource<Payperiod>;

  constructor(
    private payperiodService: PayperiodService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadLatest();
    this.loadList();
  }

  loadLatest() {
    this.payperiodService
      .getLatest()
      .subscribe((payperiod) => (this.latestPayperiod = payperiod));
  }

  loadList() {
    this.payperiodService.list().subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.items);
    });
  }

  startPayroll() {
    this.dialog
      .open(StartPayrollDialogComponent)
      .afterClosed()
      .pipe(filter((t) => t != null))
      .pipe(
        mergeMap(({ cutoffStart, cutoffEnd }) =>
          this.payperiodService.start(cutoffStart, cutoffEnd)
        )
      )
      .subscribe({
        next: () => {},
      });
  }
}
