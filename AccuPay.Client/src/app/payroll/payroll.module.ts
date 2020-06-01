import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PayrollComponent } from './payroll/payroll.component';
import { ViewPayperiodComponent } from './view-payperiod/view-payperiod.component';

@NgModule({
  declarations: [PayrollComponent, ViewPayperiodComponent],
  imports: [SharedModule],
})
export class PayrollModule {}
