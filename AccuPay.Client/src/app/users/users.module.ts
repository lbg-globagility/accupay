import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserListComponent } from 'src/app/users/user-list/user-list.component';
import { ViewUserComponent } from './view-user/view-user.component';

@NgModule({
  declarations: [UserListComponent, ViewUserComponent],
  imports: [SharedModule],
})
export class UsersModule {}
