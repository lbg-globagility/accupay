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
import { SalaryListComponent } from 'src/app/salaries/salary-list/salary-list.component';
import { ViewSalaryComponent } from 'src/app/salaries/view-salary/view-salary.component';
import { NewSalaryComponent } from 'src/app/salaries/new-salary/new-salary.component';
import { EditSalaryComponent } from 'src/app/salaries/edit-salary/edit-salary.component';
import { LeaveListComponent } from 'src/app/leaves/leave-list/leave-list.component';
import { ViewLeaveComponent } from 'src/app/leaves/view-leave/view-leave.component';
import { NewLeaveComponent } from 'src/app/leaves/new-leave/new-leave.component';
import { EditLeaveComponent } from 'src/app/leaves/edit-leave/edit-leave.component';
import { OfficialBusinessListComponent } from 'src/app/official-businesses/official-business-list/official-business-list.component';
import { ViewOfficialBusinessComponent } from 'src/app/official-businesses/view-official-business/view-official-business.component';
import { NewOfficialBusinessComponent } from 'src/app/official-businesses/new-official-business/new-official-business.component';
import { EditOfficialBusinessComponent } from 'src/app/official-businesses/edit-official-business/edit-official-business.component';
import { OvertimeListComponent } from 'src/app/overtimes/overtime-list/overtime-list.component';
import { ViewOvertimeComponent } from 'src/app/overtimes/view-overtime/view-overtime.component';
import { NewOvertimeComponent } from 'src/app/overtimes/new-overtime/new-overtime.component';
import { EditOvertimeComponent } from 'src/app/overtimes/edit-overtime/edit-overtime.component';
import { ShiftListComponent } from 'src/app/shifts/shift-list/shift-list.component';
import { ViewShiftComponent } from 'src/app/shifts/view-shift/view-shift.component';
import { NewShiftComponent } from 'src/app/shifts/new-shift/new-shift.component';
import { EditShiftComponent } from 'src/app/shifts/edit-shift/edit-shift.component';
import { AllowanceListComponent } from 'src/app/allowances/allowance-list/allowance-list.component';
import { LoanListComponent } from 'src/app/loans/loan-list/loan-list.component';
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
        path: 'salaries',
        component: SalaryListComponent,
      },
      {
        path: 'salaries/new',
        component: NewSalaryComponent,
      },
      {
        path: 'salaries/:id',
        component: ViewSalaryComponent,
      },
      {
        path: 'salaries/:id/edit',
        component: EditSalaryComponent,
      },
      {
        path: 'leaves',
        component: LeaveListComponent,
      },
      {
        path: 'leaves/new',
        component: NewLeaveComponent,
      },
      {
        path: 'leaves/:id',
        component: ViewLeaveComponent,
      },
      {
        path: 'leaves/:id/edit',
        component: EditLeaveComponent,
      },
      {
        path: 'official-businesses',
        component: OfficialBusinessListComponent,
      },
      {
        path: 'official-businesses/new',
        component: NewOfficialBusinessComponent,
      },
      {
        path: 'official-businesses/:id',
        component: ViewOfficialBusinessComponent,
      },
      {
        path: 'official-businesses/:id/edit',
        component: EditOfficialBusinessComponent,
      },
      {
        path: 'overtimes',
        component: OvertimeListComponent,
      },
      {
        path: 'overtimes/new',
        component: NewOvertimeComponent,
      },
      {
        path: 'overtimes/:id',
        component: ViewOvertimeComponent,
      },
      {
        path: 'overtimes/:id/edit',
        component: EditOvertimeComponent,
      },
      {
        path: 'shifts',
        component: ShiftListComponent,
      },
      {
        path: 'shifts/new',
        component: NewShiftComponent,
      },
      {
        path: 'shifts/:id',
        component: ViewShiftComponent,
      },
      {
        path: 'shifts/:id/edit',
        component: EditShiftComponent,
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
