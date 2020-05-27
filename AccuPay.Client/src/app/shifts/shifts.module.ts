import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ShiftListComponent } from './shift-list/shift-list.component';
import { ViewShiftComponent } from './view-shift/view-shift.component';
import { NewShiftComponent } from './new-shift/new-shift.component';
import { EditShiftComponent } from './edit-shift/edit-shift.component';
import { ShiftFormComponent } from './shift-form/shift-form.component';



@NgModule({
  declarations: [ShiftListComponent, ViewShiftComponent, NewShiftComponent, EditShiftComponent, ShiftFormComponent],
  imports: [SharedModule]
})
export class ShiftsModule { }
