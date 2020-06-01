import { Component, OnInit } from '@angular/core';
import { PayperiodService } from 'src/app/payroll/services/payperiod.service';
import { ActivatedRoute } from '@angular/router';
import { Payperiod } from 'src/app/payroll/shared/payperiod';
import { MatTableDataSource } from '@angular/material/table';
import { Paystub } from 'src/app/payroll/shared/paystub';

@Component({
  selector: 'app-view-payperiod',
  templateUrl: './view-payperiod.component.html',
  styleUrls: ['./view-payperiod.component.scss'],
})
export class ViewPayperiodComponent implements OnInit {
  private payperiodId: number = +this.route.snapshot.paramMap.get('id');

  payperiod: Payperiod;

  paystubs: Paystub;

  readonly displayedColumns = ['employee', 'netPay'];

  dataSource: MatTableDataSource<any>;

  constructor(
    private payperiodService: PayperiodService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadPayperiod();
    this.loadPaystubs();
  }

  loadPayperiod() {
    this.payperiodService
      .getById(this.payperiodId)
      .subscribe((payperiod) => (this.payperiod = payperiod));
  }

  loadPaystubs() {
    this.payperiodService
      .getPaystubs(this.payperiodId)
      .subscribe((paystubs) => (this.paystubs = paystubs));
  }

  calculate() {}
}
