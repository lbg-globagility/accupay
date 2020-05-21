import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LeaveListComponent } from './leave-list/leave-list.component';
import { ViewLeaveComponent } from './view-leave/view-leave.component';
import { EditLeaveComponent } from './edit-leave/edit-leave.component';
import { LeaveFormComponent } from './leave-form/leave-form.component';
import { NewLeaveComponent } from './new-leave/new-leave.component';

@NgModule({
  declarations: [
    LeaveListComponent,
    ViewLeaveComponent,
    EditLeaveComponent,
    LeaveFormComponent,
    NewLeaveComponent,
  ],
  imports: [SharedModule],
})
export class LeavesModule {}
