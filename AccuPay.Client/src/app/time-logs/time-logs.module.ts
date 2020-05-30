import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { TimeLogListComponent } from './time-log-list/time-log-list.component';

@NgModule({
  declarations: [TimeLogListComponent],
  imports: [SharedModule],
})
export class TimeLogsModule {}
