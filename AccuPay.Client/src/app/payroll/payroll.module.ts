import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PayrollComponent } from './payroll/payroll.component';
import { ViewPayPeriodComponent } from './view-payperiod/view-payperiod.component';
import { StartPayrollDialogComponent } from './components/start-payroll-dialog/start-payroll-dialog.component';
import { PayrollResultDetailsComponent } from './components/payroll-result-details/payroll-result-details.component';
import { PaystubDetailsComponent } from './components/paystub-details/paystub-details.component';
import { LoansBreakdownComponent } from './components/loans-breakdown/loans-breakdown.component';
import { AdjustmentsBreakdownComponent } from './components/adjustments-breakdown/adjustments-breakdown.component';
import { SelectPayperiodDialogComponent } from './components/select-payperiod-dialog/select-payperiod-dialog.component';

@NgModule({
  declarations: [
    PayrollComponent,
    ViewPayPeriodComponent,
    StartPayrollDialogComponent,
    PayrollResultDetailsComponent,
    PaystubDetailsComponent,
    LoansBreakdownComponent,
    AdjustmentsBreakdownComponent,
    SelectPayperiodDialogComponent,
  ],
  imports: [SharedModule],
})
export class PayrollModule {}
