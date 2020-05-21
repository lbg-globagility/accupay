import { NgModule } from '@angular/core';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NewOrganizationComponent } from './new-organization/new-organization.component';
import { OrganizationFormComponent } from './organization-form/organization-form.component';
import { ViewOrganizationComponent } from './view-organization/view-organization.component';
import { EditOrganizationComponent } from './edit-organization/edit-organization.component';

@NgModule({
  declarations: [OrganizationListComponent, NewOrganizationComponent, OrganizationFormComponent, ViewOrganizationComponent, EditOrganizationComponent],
  imports: [SharedModule],
})
export class OrganizationsModule {}
