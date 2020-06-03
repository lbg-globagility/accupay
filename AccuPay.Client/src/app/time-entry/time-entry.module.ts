import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { TimeEntryComponent } from './time-entry/time-entry.component';



@NgModule({
  declarations: [TimeEntryComponent],
  imports: [SharedModule]
})
export class TimeEntryModule { }
