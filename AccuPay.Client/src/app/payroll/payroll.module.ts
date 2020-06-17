import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PayrollComponent } from './payroll/payroll.component';
import { ViewPayPeriodComponent } from './view-payperiod/view-payperiod.component';
import { StartPayrollDialogComponent } from './start-payroll-dialog/start-payroll-dialog.component';
import { PayrollResultDetailsComponent } from './payroll-result-details/payroll-result-details.component';

@NgModule({
  declarations: [
    PayrollComponent,
    ViewPayPeriodComponent,
    StartPayrollDialogComponent,
    PayrollResultDetailsComponent,
  ],
  imports: [SharedModule],
})
export class PayrollModule {}
