import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { OvertimeListComponent } from './overtime-list/overtime-list.component';



@NgModule({
  declarations: [OvertimeListComponent],
  imports: [SharedModule]
})
export class OvertimesModule { }
