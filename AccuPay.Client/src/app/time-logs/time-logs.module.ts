import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { TimeLogListComponent } from './time-log-list/time-log-list.component';
import { ViewTimeLogComponent } from './view-time-log/view-time-log.component';
import { EditTimeLogComponent } from './edit-time-log/edit-time-log.component';
import { TimeLogFormComponent } from './time-log-form/time-log-form.component';
import { NewTimeLogComponent } from './new-time-log/new-time-log.component';
import { TimeLogs2Component } from './time-logs2/time-logs2.component';

@NgModule({
  declarations: [TimeLogListComponent, ViewTimeLogComponent, EditTimeLogComponent, TimeLogFormComponent, NewTimeLogComponent, TimeLogs2Component],
  imports: [SharedModule],
})
export class TimeLogsModule {}
