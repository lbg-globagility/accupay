import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import {
  NewClientComponent,
  ClientFormComponent,
  EditClientComponent,
  ClientsComponent,
  ViewClientComponent,
  UserFormComponent,
} from 'src/app/clients/components';
import { OrganizationFormComponent } from './components/organization-form/organization-form.component';

@NgModule({
  declarations: [
    NewClientComponent,
    ClientFormComponent,
    EditClientComponent,
    ClientsComponent,
    ViewClientComponent,
    UserFormComponent,
    OrganizationFormComponent,
  ],
  imports: [SharedModule],
})
export class ClientsModule {}
