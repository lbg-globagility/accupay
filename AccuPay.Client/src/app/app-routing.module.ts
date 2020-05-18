import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from 'src/app/main/main.component';
import { PageNotFoundComponent } from 'src/app/errors/page-not-found/page-not-found.component';
import { UnderConstructionComponent } from 'src/app/errors/under-construction/under-construction.component';
import { LoginComponent } from 'src/app/accounts/login/login.component';
import { AuthGuard } from 'src/app/core/auth/auth.guard';
import { EmployeeListComponent } from './employees/employee-list/employee-list.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'employees',
        component: EmployeeListComponent,
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
