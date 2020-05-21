import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from 'src/app/main/main.component';
import { PageNotFoundComponent } from 'src/app/errors/page-not-found/page-not-found.component';
import { UnderConstructionComponent } from 'src/app/errors/under-construction/under-construction.component';
import { LoginComponent } from 'src/app/accounts/login/login.component';
import { AuthGuard } from 'src/app/core/auth/auth.guard';
import { EmployeeListComponent } from './employees/employee-list/employee-list.component';
import { UserListComponent } from 'src/app/users/user-list/user-list.component';
import { ViewUserComponent } from 'src/app/users/view-user/view-user.component';
import { NewUserComponent } from 'src/app/users/new-user/new-user.component';
import { EditUserComponent } from 'src/app/users/edit-user/edit-user.component';
import { NewEmployeeComponent } from './employees/new-employee/new-employee.component';
import { ViewEmployeeComponent } from './employees/view-employee/view-employee.component';
import { EditEmployeeComponent } from './employees/edit-employee/edit-employee.component';
import { AllowanceListComponent } from 'src/app/allowances/allowance-list/allowance-list.component';
import { LeaveListComponent } from 'src/app/leaves/leave-list/leave-list.component';
import { LoanListComponent } from 'src/app/loans/loan-list/loan-list.component';
import { OfficialBusinessListComponent } from 'src/app/official-businesses/official-business-list/official-business-list.component';
import { OvertimeListComponent } from 'src/app/overtimes/overtime-list/overtime-list.component';
import { OrganizationListComponent } from 'src/app/organizations/organization-list/organization-list.component';
import { NewOrganizationComponent } from 'src/app/organizations/new-organization/new-organization.component';
import { ViewOrganizationComponent } from 'src/app/organizations/view-organization/view-organization.component';
import { EditOrganizationComponent } from 'src/app/organizations/edit-organization/edit-organization.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'employees/new',
        component: NewEmployeeComponent,
      },
      {
        path: 'employees/:id',
        component: ViewEmployeeComponent,
      },
      {
        path: 'employees/:id/edit',
        component: EditEmployeeComponent,
      },
      {
        path: 'employees',
        component: EmployeeListComponent,
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
      {
        path: 'leaves',
        component: LeaveListComponent,
      },
      {
        path: 'official-business',
        component: OfficialBusinessListComponent,
      },
      {
        path: 'overtimes',
        component: OvertimeListComponent,
      },
      {
        path: 'allowances',
        component: AllowanceListComponent,
      },
      {
        path: 'loans',
        component: LoanListComponent,
      },
      {
        path: 'organizations',
        component: OrganizationListComponent,
      },
      {
        path: 'organizations/new',
        component: NewOrganizationComponent,
      },
      {
        path: 'organizations/:id',
        component: ViewOrganizationComponent,
      },
      {
        path: 'organizations/:id/edit',
        component: EditOrganizationComponent,
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
