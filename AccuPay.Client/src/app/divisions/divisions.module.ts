import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { DivisionListComponent } from './division-list/division-list.component';
import { ViewDivisionComponent } from './view-division/view-division.component';
import { EditDivisionComponent } from './edit-division/edit-division.component';
import { NewDivisionComponent } from './new-division/new-division.component';
import { DivisionFormComponent } from './division-form/division-form.component';

@NgModule({
  declarations: [
    ViewDivisionComponent,
    DivisionListComponent,
    EditDivisionComponent,
    NewDivisionComponent,
    DivisionFormComponent,
  ],
  imports: [SharedModule],
})
export class DivisionsModule {}
