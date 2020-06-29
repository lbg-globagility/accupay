import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LeaveListComponent } from './leave-list/leave-list.component';
import { ViewLeaveComponent } from './view-leave/view-leave.component';
import { EditLeaveComponent } from './edit-leave/edit-leave.component';
import { LeaveFormComponent } from './leave-form/leave-form.component';
import { NewLeaveComponent } from './new-leave/new-leave.component';
import { LeaveBalanceComponent } from './leave-balance/leave-balance.component';
import { LeaveLedgerComponent } from './leave-ledger/leave-ledger.component';
import { LeavesComponent } from './leaves/leaves.component';

@NgModule({
  declarations: [
    LeaveListComponent,
    ViewLeaveComponent,
    EditLeaveComponent,
    LeaveFormComponent,
    NewLeaveComponent,
    LeaveBalanceComponent,
    LeaveLedgerComponent,
    LeavesComponent,
  ],
  imports: [SharedModule],
})
export class LeavesModule {}
