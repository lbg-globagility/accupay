import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { EditTimeLogComponent } from './edit-time-log/edit-time-log.component';
import { TimeLogImportResultComponent } from './time-log-import-result/time-log-import-result.component';
import { TimeLogsComponent } from './time-logs/time-logs.component';

@NgModule({
  declarations: [
    EditTimeLogComponent,
    TimeLogsComponent,
    TimeLogImportResultComponent,
  ],
  imports: [SharedModule],
})
export class TimeLogsModule {}
