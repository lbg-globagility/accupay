import { Component, OnInit } from '@angular/core';
import { Payperiod } from 'src/app/payroll/shared/payperiod';
import { PayperiodService } from 'src/app/payroll/services/payperiod.service';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
})
export class PayrollComponent implements OnInit {
  latestPayperiod: Payperiod;

  readonly displayedColumns = ['cutoff', 'status'];

  dataSource: MatTableDataSource<Payperiod>;

  constructor(private payperiodService: PayperiodService) {}

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
}
