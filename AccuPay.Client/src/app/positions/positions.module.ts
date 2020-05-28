import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PositionListComponent } from './position-list/position-list.component';
import { ViewPositionComponent } from './view-position/view-position.component';
import { NewPositionComponent } from './new-position/new-position.component';
import { EditPositionComponent } from './edit-position/edit-position.component';
import { PositionFormComponent } from './position-form/position-form.component';

@NgModule({
  declarations: [PositionListComponent, ViewPositionComponent, NewPositionComponent, EditPositionComponent, PositionFormComponent],
  imports: [SharedModule],
})
export class PositionsModule {}
