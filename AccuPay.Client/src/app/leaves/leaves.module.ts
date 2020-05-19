import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LeaveListComponent } from './leave-list/leave-list.component';



@NgModule({
  declarations: [LeaveListComponent],
  imports: [SharedModule]
})
export class LeavesModule { }
