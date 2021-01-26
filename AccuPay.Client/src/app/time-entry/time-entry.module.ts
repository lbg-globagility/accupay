import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { TimeEntryComponent } from './time-entry/time-entry.component';
import { TimeEntryDetailsComponent } from './time-entry-details/time-entry-details.component';
import { TimeEntrySummaryDetailsComponent } from './components/time-entry-summary-details/time-entry-summary-details.component';
import { TimeEntryTableComponent } from './time-entry-table/time-entry-table.component';

@NgModule({
  declarations: [
    TimeEntryComponent,
    TimeEntryDetailsComponent,
    TimeEntrySummaryDetailsComponent,
    TimeEntryTableComponent,
  ],
  imports: [SharedModule],
  exports: [TimeEntryTableComponent],
})
export class TimeEntryModule {}
