import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReportsComponent } from '../reports/reports/reports.component';
import { SelectMonthComponent } from './components/select-month/select-month.component';
import { SssReportComponent } from './sss-report/sss-report.component';
import { PhilhealthReportComponent } from './philhealth-report/philhealth-report.component';
import { PagibigReportComponent } from './pagibig-report/pagibig-report.component';
import { LoanReportBytypeComponent } from './loan-report-bytype/loan-report-bytype.component';
import { SelectDateRangeComponent } from './components/select-date-range/select-date-range.component';
import { LoanReportByemployeeComponent } from './loan-report-byemployee/loan-report-byemployee.component';
import { TaxReportComponent } from './tax-report/tax-report.component';
import { ThirtheenthMonthReportComponent } from './thirtheenth-month-report/thirtheenth-month-report.component';
import { PayrollSummaryComponent } from './payroll-summary/payroll-summary.component';

@NgModule({
  declarations: [
    ReportsComponent,
    SelectMonthComponent,
    SssReportComponent,
    PhilhealthReportComponent,
    PagibigReportComponent,
    LoanReportBytypeComponent,
    SelectDateRangeComponent,
    LoanReportByemployeeComponent,
    TaxReportComponent,
    ThirtheenthMonthReportComponent,
    PayrollSummaryComponent,
  ],

  imports: [SharedModule],
})
export class ReportsModule {}
