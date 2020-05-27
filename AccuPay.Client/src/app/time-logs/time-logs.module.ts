import { NgModule } from '@angular/core';
import { TimeLogTableComponent } from './time-log-table/time-log-table.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [TimeLogTableComponent],
  imports: [SharedModule],
})
export class TimeLogsModule {}
