import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from 'src/app/main/main.component';
import { PageNotFoundComponent } from 'src/app/errors/page-not-found/page-not-found.component';
import { UnderConstructionComponent } from 'src/app/errors/under-construction/under-construction.component';
import { LoginComponent } from 'src/app/accounts/login/login.component';
import { AuthGuard } from 'src/app/core/auth/auth.guard';
import { UserListComponent } from 'src/app/users/user-list/user-list.component';
import { ViewUserComponent } from 'src/app/users/view-user/view-user.component';
import { NewUserComponent } from 'src/app/users/new-user/new-user.component';
import { EditUserComponent } from 'src/app/users/edit-user/edit-user.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'employees',
        component: UnderConstructionComponent,
      },
      {
        path: 'users',
        component: UserListComponent,
      },
      {
        path: 'users/new',
        component: NewUserComponent,
      },
      {
        path: 'users/:id',
        component: ViewUserComponent,
      },
      {
        path: 'users/:id/edit',
        component: EditUserComponent,
      },
    ],
    canActivate: [AuthGuard],
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
