import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { NewClientComponent } from './components/new-client/new-client.component';
import { ClientFormComponent } from './components/client-form/client-form.component';
import { EditClientComponent } from './components/edit-client/edit-client.component';
import { ClientsComponent } from './components/clients/clients.component';
import { ViewClientComponent } from './components/view-client/view-client.component';

@NgModule({
  declarations: [NewClientComponent, ClientFormComponent, EditClientComponent, ClientsComponent, ViewClientComponent],
  imports: [SharedModule],
})
export class ClientsModule {}
