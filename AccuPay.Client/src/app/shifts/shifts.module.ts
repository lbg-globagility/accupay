import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ShiftListComponent } from './shift-list/shift-list.component';
import { EditShiftComponent } from './edit-shift/edit-shift.component';

@NgModule({
  declarations: [ShiftListComponent, EditShiftComponent],
  imports: [SharedModule],
})
export class ShiftsModule {}
