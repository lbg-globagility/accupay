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

@NgModule({
  declarations: [
    NewClientComponent,
    ClientFormComponent,
    EditClientComponent,
    ClientsComponent,
    ViewClientComponent,
    UserFormComponent,
  ],
  imports: [SharedModule],
})
export class ClientsModule {}
