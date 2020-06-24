import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReportsComponent } from '../reports/reports/reports.component';
import { ReportFormComponent } from './report-form/report-form.component';

@NgModule({
  declarations: [ReportsComponent, ReportFormComponent],
  imports: [SharedModule],
})
export class ReportsModule {}
