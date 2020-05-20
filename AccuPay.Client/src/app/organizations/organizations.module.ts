import { NgModule } from '@angular/core';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [OrganizationListComponent],
  imports: [SharedModule],
})
export class OrganizationsModule {}
