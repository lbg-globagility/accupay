import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LeaveListComponent } from './leave-list/leave-list.component';
import { ViewLeaveComponent } from './view-leave/view-leave.component';

@NgModule({
  declarations: [LeaveListComponent, ViewLeaveComponent],
  imports: [SharedModule],
})
export class LeavesModule {}
