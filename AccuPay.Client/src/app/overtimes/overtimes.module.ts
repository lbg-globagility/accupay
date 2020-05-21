import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { OvertimeListComponent } from './overtime-list/overtime-list.component';
import { ViewOvertimeComponent } from './view-overtime/view-overtime.component';



@NgModule({
  declarations: [OvertimeListComponent, ViewOvertimeComponent],
  imports: [SharedModule]
})
export class OvertimesModule { }
