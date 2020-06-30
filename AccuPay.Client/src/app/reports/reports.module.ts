import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReportsComponent } from '../reports/reports/reports.component';
import { SelectMonthComponent } from './components/select-month/select-month.component';
import { SssReportComponent } from './sss-report/sss-report.component';

@NgModule({
  declarations: [ReportsComponent, SelectMonthComponent, SssReportComponent],
  imports: [SharedModule],
})
export class ReportsModule {}
