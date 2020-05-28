import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PositionListComponent } from './position-list/position-list.component';
import { ViewPositionComponent } from './view-position/view-position.component';

@NgModule({
  declarations: [PositionListComponent, ViewPositionComponent],
  imports: [SharedModule],
})
export class PositionsModule {}
