import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PayrollComponent } from './payroll/payroll.component';
import { ViewPayperiodComponent } from './view-payperiod/view-payperiod.component';
import { StartPayrollDialogComponent } from './start-payroll-dialog/start-payroll-dialog.component';

@NgModule({
  declarations: [PayrollComponent, ViewPayperiodComponent, StartPayrollDialogComponent],
  imports: [SharedModule],
})
export class PayrollModule {}
