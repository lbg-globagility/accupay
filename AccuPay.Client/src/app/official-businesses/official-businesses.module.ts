import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { OfficialBusinessListComponent } from './official-business-list/official-business-list.component';
import { NewOfficialBusinessComponent } from './new-official-business/new-official-business.component';
import { EditOfficialBusinessComponent } from './edit-official-business/edit-official-business.component';
import { OfficialBusinessFormComponent } from './official-business-form/official-business-form.component';

@NgModule({
  declarations: [
    OfficialBusinessListComponent,
    NewOfficialBusinessComponent,
    EditOfficialBusinessComponent,
    OfficialBusinessFormComponent,
  ],
  imports: [SharedModule],
})
export class OfficialBusinessesModule {}
