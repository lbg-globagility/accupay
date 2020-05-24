import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { OvertimeListComponent } from './overtime-list/overtime-list.component';
import { ViewOvertimeComponent } from './view-overtime/view-overtime.component';
import { NewOvertimeComponent } from './new-overtime/new-overtime.component';
import { EditOvertimeComponent } from './edit-overtime/edit-overtime.component';
import { OvertimeFormComponent } from './overtime-form/overtime-form.component';



@NgModule({
  declarations: [OvertimeListComponent, ViewOvertimeComponent, NewOvertimeComponent, EditOvertimeComponent, OvertimeFormComponent],
  imports: [SharedModule]
})
export class OvertimesModule { }
