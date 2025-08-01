import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { RoleFormComponent } from './role-form/role-form.component';
import { NewRoleComponent } from './new-role/new-role.component';
import { RoleListComponent } from './role-list/role-list.component';
import { EditRoleComponent } from './edit-role/edit-role.component';
import { UserRolesComponent } from './user-roles/user-roles.component';

@NgModule({
  declarations: [
    RoleFormComponent,
    NewRoleComponent,
    RoleListComponent,
    EditRoleComponent,
    UserRolesComponent,
  ],
  imports: [SharedModule],
})
export class RolesModule {}
