import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PositionListComponent } from './position-list/position-list.component';

@NgModule({
  declarations: [PositionListComponent],
  imports: [SharedModule],
})
export class PositionsModule {}
