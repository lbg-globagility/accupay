import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { OfficialBusinessListComponent } from './official-business-list/official-business-list.component';



@NgModule({
  declarations: [OfficialBusinessListComponent],
  imports: [
    SharedModule
  ]
})
export class OfficialBusinessesModule { }
