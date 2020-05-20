import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserListComponent } from 'src/app/users/user-list/user-list.component';
import { ViewUserComponent } from './view-user/view-user.component';
import { UserFormComponent } from './user-form/user-form.component';
import { NewUserComponent } from './new-user/new-user.component';
import { EditUserComponent } from './edit-user/edit-user.component';

@NgModule({
  declarations: [UserListComponent, ViewUserComponent, UserFormComponent, NewUserComponent, EditUserComponent],
  imports: [SharedModule],
})
export class UsersModule {}
