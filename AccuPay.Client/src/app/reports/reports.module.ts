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
import { ThirteenthMonthReportComponent } from './thirteenth-month-report/thirteenth-month-report.component';
import { PayrollSummaryComponent } from './payroll-summary/payroll-summary.component';
import { SelectPayperiodRangeDialogComponent } from './components/select-payperiod-range-dialog/select-payperiod-range-dialog.component';

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
    ThirteenthMonthReportComponent,
    PayrollSummaryComponent,
    SelectPayperiodRangeDialogComponent,
  ],

  imports: [SharedModule],
})
export class ReportsModule {}
