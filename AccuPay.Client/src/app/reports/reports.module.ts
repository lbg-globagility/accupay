import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoanReportComponent } from '../reports/loan-report/loan-report.component';

@NgModule({
  declarations: [LoanReportComponent],
  imports: [SharedModule],
})
export class ReportsModule {}
