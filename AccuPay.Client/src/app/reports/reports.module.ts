import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReportsComponent } from '../reports/reports/reports.component';
import { SelectMonthComponent } from './components/select-month/select-month.component';
import { SssReportComponent } from './sss-report/sss-report.component';
import { PhilhealthReportComponent } from './philhealth-report/philhealth-report.component';
import { PagibigReportComponent } from './pagibig-report/pagibig-report.component';

@NgModule({
  declarations: [
    ReportsComponent,
    SelectMonthComponent,
    SssReportComponent,
    PhilhealthReportComponent,
    PagibigReportComponent,
  ],

  imports: [SharedModule],
})
export class ReportsModule {}
